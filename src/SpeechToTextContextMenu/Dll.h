#pragma once

#include "ShellHelpers.h"
#include "RegisterExtension.h"
#include <strsafe.h>
#include <new>

void DllAddRef();
void DllRelease();

class __declspec(uuid("08DAC079-F299-447B-9550-2670E7C5501B")) SpeechToTextGenerateSrtCommand;
class __declspec(uuid("08DAC079-F299-447B-9550-2670E7C5501C")) SpeechToTextGenerateTranslatedSrtCommand;

HRESULT SpeechToTextGenerateSrtCommand_CreateInstance(REFIID riid, void **ppv);
HRESULT SpeechToTextGenerateSrtCommand_RegisterUnRegister(bool fRegister);

HRESULT SpeechToTextGenerateTranslatedSrtCommand_CreateInstance(REFIID riid, void **ppv);
HRESULT SpeechToTextGenerateTranslatedSrtCommand_RegisterUnRegister(bool fRegister);
