#include "Dll.h"
#include <iostream>
#include <Windows.h>

static WCHAR const c_szVerbDisplayName[] = L"Generate SRT";
static WCHAR const c_szVerbName[] = L"SpeechToTextGenerateSrtCommand";
static WCHAR const c_szProgID[] = L"txtfile";

class SpeechToTextGenerateSrtCommand : public IExplorerCommand,
                                       public IInitializeCommand,
                                       public IObjectWithSite
{
public:
    SpeechToTextGenerateSrtCommand() : _cRef(1), _punkSite(NULL), _hwnd(NULL), _pstmShellItemArray(NULL)
    {
        DllAddRef();
    }

    // IUnknown
    IFACEMETHODIMP QueryInterface(REFIID riid, void **ppv)
    {
        static const QITAB qit[] =
            {
                QITABENT(SpeechToTextGenerateSrtCommand, IExplorerCommand),
                QITABENT(SpeechToTextGenerateSrtCommand, IInitializeCommand),
                QITABENT(SpeechToTextGenerateSrtCommand, IObjectWithSite),
                {0},
            };

        return QISearch(this, qit, riid, ppv);
    }

    IFACEMETHODIMP_(ULONG)
    AddRef()
    {
        return InterlockedIncrement(&_cRef);
    }

    IFACEMETHODIMP_(ULONG)
    Release()
    {
        long cRef = InterlockedDecrement(&_cRef);

        if (!cRef)
        {
            delete this;
        }

        return cRef;
    }

    // IExplorerCommand

    IFACEMETHODIMP GetTitle(IShellItemArray * /* psiItemArray */, LPWSTR *ppszName)
    {
        return SHStrDup(c_szVerbDisplayName, ppszName);
    }

    IFACEMETHODIMP GetIcon(IShellItemArray * /* psiItemArray */, LPWSTR *ppszIcon)
    {
        *ppszIcon = NULL;

        return E_NOTIMPL;
    }

    IFACEMETHODIMP GetToolTip(IShellItemArray * /* psiItemArray */, LPWSTR *ppszInfotip)
    {
        *ppszInfotip = NULL;

        return E_NOTIMPL;
    }

    IFACEMETHODIMP GetCanonicalName(GUID *pguidCommandName)
    {
        *pguidCommandName = __uuidof(this);

        return S_OK;
    }

    IFACEMETHODIMP GetState(IShellItemArray * /* psiItemArray */, BOOL /* fOkToBeSlow */, EXPCMDSTATE * /* pCmdState */)
    {
        return S_OK;
    }

    IFACEMETHODIMP Invoke(IShellItemArray *psiItemArray, IBindCtx *pbc);

    IFACEMETHODIMP GetFlags(EXPCMDFLAGS *pFlags)
    {
        *pFlags = ECF_DEFAULT;

        return S_OK;
    }

    IFACEMETHODIMP EnumSubCommands(IEnumExplorerCommand **ppEnum)
    {
        *ppEnum = NULL;

        return E_NOTIMPL;
    }

    // IInitializeCommand

    IFACEMETHODIMP Initialize(PCWSTR /* pszCommandName */, IPropertyBag * /* ppb */)
    {
        return S_OK;
    }

    // IObjectWithSite

    IFACEMETHODIMP SetSite(IUnknown *punkSite)
    {
        SetInterface(&_punkSite, punkSite);

        return S_OK;
    }

    IFACEMETHODIMP GetSite(REFIID riid, void **ppv)
    {
        *ppv = NULL;

        return _punkSite ? _punkSite->QueryInterface(riid, ppv) : E_FAIL;
    }

private:
    ~SpeechToTextGenerateSrtCommand()
    {
        SafeRelease(&_punkSite);
        SafeRelease(&_pstmShellItemArray);
        DllRelease();
    }

    long _cRef;
    IUnknown *_punkSite;
    HWND _hwnd;
    IStream *_pstmShellItemArray;
};

IFACEMETHODIMP SpeechToTextGenerateSrtCommand::Invoke(IShellItemArray *psia, IBindCtx * /* pbc */)
{
    IUnknown_GetWindow(_punkSite, &_hwnd);

    DWORD count;
    HRESULT hr = psia->GetCount(&count);

    if (SUCCEEDED(hr))
    {
        WCHAR szDllPath[MAX_PATH];
        GetModuleFileName(GetModuleHINSTANCE(), szDllPath, ARRAYSIZE(szDllPath));
        PathRemoveFileSpec(szDllPath);
        PathRemoveFileSpec(szDllPath);

        PathAppend(szDllPath, L"SpeechToTextCli\\SpeechToTextCli.exe");

        for (DWORD i = 0; i < count; i++)
        {
            IShellItem *psi;
            hr = psia->GetItemAt(i, &psi);

            if (SUCCEEDED(hr))
            {
                PWSTR pszFilePath;
                hr = psi->GetDisplayName(SIGDN_FILESYSPATH, &pszFilePath);

                if (SUCCEEDED(hr))
                {
                    WCHAR szParameters[MAX_PATH];
                    StringCchPrintf(szParameters, ARRAYSIZE(szParameters), L"gs --file \"%s\"", pszFilePath);

                    SHELLEXECUTEINFO sei = {sizeof(sei)};
                    sei.fMask = SEE_MASK_NOCLOSEPROCESS;
                    sei.hwnd = _hwnd;
                    sei.lpVerb = L"open";
                    sei.lpFile = szDllPath;
                    sei.lpParameters = szParameters;
                    sei.nShow = SW_HIDE;

                    if (!ShellExecuteEx(&sei))
                    {
                        hr = HRESULT_FROM_WIN32(GetLastError());
                    }

                    CoTaskMemFree(pszFilePath);
                }

                psi->Release();
            }
        }
    }

    return hr;
}

HRESULT SpeechToTextGenerateSrtCommand_RegisterUnRegister(bool fRegister)
{
    CRegisterExtension re(__uuidof(SpeechToTextGenerateSrtCommand));

    HRESULT hr;

    if (fRegister)
    {
        hr = re.RegisterInProcServer(c_szVerbDisplayName, L"Apartment");

        if (SUCCEEDED(hr))
        {
            hr = re.RegisterExplorerCommandVerb(c_szProgID, c_szVerbName, c_szVerbDisplayName);

            if (SUCCEEDED(hr))
            {
                hr = re.RegisterVerbAttribute(c_szProgID, c_szVerbName, L"NeverDefault");
            }
        }
    }
    else
    {
        hr = re.UnRegisterVerb(c_szProgID, c_szVerbName);
        hr = re.UnRegisterObject();
    }

    return hr;
}

HRESULT SpeechToTextGenerateSrtCommand_CreateInstance(REFIID riid, void **ppv)
{
    *ppv = NULL;
    SpeechToTextGenerateSrtCommand *pVerb = new (std::nothrow) SpeechToTextGenerateSrtCommand();
    HRESULT hr = pVerb ? S_OK : E_OUTOFMEMORY;

    if (SUCCEEDED(hr))
    {
        pVerb->QueryInterface(riid, ppv);
        pVerb->Release();
    }

    return hr;
}
