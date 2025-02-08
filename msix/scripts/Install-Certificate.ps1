<#
.SYNOPSIS
    Installs the certificate for the SpeechToTextCli application.
.DESCRIPTION
    This script installs the certificate required for the installation of the SpeechToTextCli application.
    Requires administrator privileges.
#>

# Check for administrator privileges
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Error "This script requires administrator privileges. Please run PowerShell as an administrator and try again."
    pause
    exit 1
}

try {
    # Path to the certificate (relative to the script location)
    $certPath = Join-Path $PSScriptRoot "SpeechToTextCli.cer"
    
    # Check if the certificate file exists
    if (-not (Test-Path $certPath)) {
        Write-Error "Certificate file not found at: $certPath"
        pause
        exit 1
    }
    
    Write-Host "Installing certificate..." -ForegroundColor Yellow
    
    # Install the certificate
    Import-Certificate -FilePath $certPath -CertStoreLocation "Cert:\LocalMachine\Root"
    
    Write-Host "`nCertificate successfully installed." -ForegroundColor Green
    Write-Host "You can now install the MSIX application." -ForegroundColor Green
}
catch {
    Write-Error "An error occurred during certificate installation: $_"
    pause
    exit 1
}

Write-Host "`nPress any key to exit..."
pause