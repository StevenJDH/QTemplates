/**
 * This file is part of QTemplates <https://github.com/StevenJDH/QTemplates>.
 * Copyright (C) 2019 Steven Jenkins De Haro.
 *
 * QTemplates is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * QTemplates is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with QTemplates.  If not, see <http://www.gnu.org/licenses/>.
 */

  ;NSIS Modern User Interface
  ;Normal (Multi User) Script
  !define INSTALLER_VERSION 1.0

  !pragma warning error all
  SetCompressor lzma
  Unicode true
  
  ;The following defines have to appear before including "MultiUser.nsh" to work.
  !define PRODUCT_NAME "QTemplates"
  !define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  ;MULTIUSER_INSTALLMODE_DEFAULT_CURRENTUSER ;Sets default to a per-user installation, even if per-machine rights are available.
  !define MULTIUSER_INSTALLMODE_INSTDIR "${PRODUCT_NAME}"
  !define MULTIUSER_INSTALLMODE_INSTDIR_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
  !define MULTIUSER_INSTALLMODE_INSTDIR_REGISTRY_VALUENAME "InstallLocation"
  ;!define MULTIUSER_USE_PROGRAMFILES64
  !define MULTIUSER_EXECUTIONLEVEL Highest
  !define MULTIUSER_MUI
  !define MULTIUSER_INSTALLMODE_COMMANDLINE
  
;--------------------------------
;Includes 

  !include "MultiUser.nsh"
  !include "MUI2.nsh"
  !include "FileFunc.nsh"

;--------------------------------
;General

  !define PRODUCT_VERSION "1.0.0.19036"
  !define MIN_WIN_VER "7"
  !define COMPANY_NAME "Steven Jenkins De Haro"
  !define COPYRIGHT_TEXT "Copyright © 2019 ${COMPANY_NAME}"
  !define PRODUCT_WEB_SITE "https://github.com/StevenJDH/QTemplates"
  !define PRODUCT_UNINST_ROOT_KEY SHCTX

  !define CONFIG_DIRECTORY "$APPDATA\ASC-C\${PRODUCT_NAME}"
  !define SQLITE_FILE "${CONFIG_DIRECTORY}\QTemplates.sqlite3"
  
  !define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-install.ico" ;Installer icon
  !define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-uninstall.ico" ;Uninstaller icon
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "Custom Graphics\Header\150_X_57_computer.bmp" ;recommended size: 150x57 pixels
  !define MUI_HEADERIMAGE_UNBITMAP "Custom Graphics\Header\150_X_57_computer.bmp"
  !define MUI_HEADERIMAGE_RIGHT
  !define MUI_WELCOMEFINISHPAGE_BITMAP "Custom Graphics\Wizard\164_X_314_computer.bmp" ;recommended size: 164x314 pixels
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP "Custom Graphics\Wizard\164_X_314_computer.bmp"
  !define MUI_COMPONENTSPAGE_SMALLDESC ;Puts component description on the bottom.

  ;Name, title bar caption, file, and branding
  Name "${PRODUCT_NAME}"
  Caption "${PRODUCT_NAME} ${PRODUCT_VERSION}" ;Default is used if left empty or removed.
  OutFile "${PRODUCT_NAME}_Setup.exe"
  BrandingText "${COPYRIGHT_TEXT}"

  ;Installer properties
  VIFileVersion "${INSTALLER_VERSION}.0.0" ;Will use VIProductVersion if not defined. Requires x.x.x.x format.
  VIProductVersion "${PRODUCT_VERSION}" ;Requires x.x.x.x format.
  VIAddVersionKey ProductName "${PRODUCT_NAME}"
  VIAddVersionKey Comments "This program is being distributed under the terms of the GNU General Public License (GPL)."
  VIAddVersionKey CompanyName "${COMPANY_NAME}"
  VIAddVersionKey LegalCopyright "${COPYRIGHT_TEXT}"
  VIAddVersionKey FileDescription "A productivity tool that is used to inject text-based templates into any application."
  VIAddVersionKey FileVersion "${INSTALLER_VERSION}"
  VIAddVersionKey ProductVersion ${PRODUCT_VERSION}
  VIAddVersionKey InternalName "${PRODUCT_NAME}"
  VIAddVersionKey LegalTrademarks "${PRODUCT_NAME} and all logos are trademarks of ${COMPANY_NAME}."
  VIAddVersionKey OriginalFilename "${PRODUCT_NAME}.exe"
  
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  
;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "..\LICENSE"
  !insertmacro MULTIUSER_PAGE_INSTALLMODE
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_COMPONENTS
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

  InstType "Full"
  InstType "Minimal"

Section "${PRODUCT_NAME} Core Files (required)" SectionCore
  SectionIn RO ;Makes the install option required/read-only.
  
  ;Files to install...
  SetOverwrite try
  SetOutPath "$INSTDIR\Plugins"
  File "..\QTemplates\bin\Release\Plugins\QTemplates.Example.Plugin.dll"
  SetOutPath "$INSTDIR\x64"
  File "..\QTemplates\bin\Release\x64\SQLite.Interop.dll"
  SetOutPath "$INSTDIR\x86"
  File "..\QTemplates\bin\Release\x86\SQLite.Interop.dll"
  SetOutPath "$INSTDIR" ;Need to set '$INSTDIR' last so that 'Start in' for shorcuts are set correctly and app plugins load.
  File "..\QTemplates\bin\Release\EntityFramework.dll"
  File "..\QTemplates\bin\Release\EntityFramework.SqlServer.dll"
  File "..\QTemplates\bin\Release\Newtonsoft.Json.dll"
  File "..\QTemplates\bin\Release\QInterfaces.dll"
  File "..\QTemplates\bin\Release\QTemplates.exe"
  File "..\QTemplates\bin\Release\System.Data.SQLite.dll"
  File "..\QTemplates\bin\Release\System.Data.SQLite.EF6.dll"
  File "..\QTemplates\bin\Release\System.Data.SQLite.Linq.dll"
SectionEnd

Section "Start Menu Shortcut" SectionStartMenu
SectionIn 1 2

  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
  ${If} $MultiUser.InstallMode == "CurrentUser"
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME} (Current User).lnk" "$INSTDIR\QTemplates.exe"
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall (Current User).lnk" "$INSTDIR\Uninstall.exe" "/CurrentUser"
  ${Else}
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\QTemplates.exe"
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "/AllUsers"
  ${EndIf}
  
SectionEnd

Section "Desktop Shortcut" SectionDesktop
SectionIn 1

  ${If} $MultiUser.InstallMode == "CurrentUser"
    CreateShortCut "$DESKTOP\${PRODUCT_NAME} (Current User).lnk" "$INSTDIR\QTemplates.exe"
  ${Else}
    CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\QTemplates.exe"
  ${EndIf}
  
SectionEnd

Section -Post

  ;Creates uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe" 

  ${If} $MultiUser.InstallMode == "CurrentUser"
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name) (Current User)"
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe /CurrentUser"
  ${Else}
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe /AllUsers"
  ${EndIf}
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\QTemplates.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${COMPANY_NAME}"
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "EstimatedSize" "$0"
  ;The following two reg entries are for the Windows' uninstall button so it displays as Uninstall only.
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" 1
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRepair" 1
  
SectionEnd

;--------------------------------
;Descriptions for Installer

  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionCore} "The core files required to use ${PRODUCT_NAME}."
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionStartMenu} "Adds an icon to your start menu for easy access."
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionDesktop} "Adds an icon to your desktop for easy access."
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Installer Functions

Function .onInit

  ${ifnot} ${AtLeastWin${MIN_WIN_VER}}
    MessageBox MB_ICONSTOP "This program requires at least Windows ${MIN_WIN_VER}." /SD IDOK
    Quit ; will SetErrorLevel 2 - Installation aborted by script
  ${endif}
  
  !insertmacro MULTIUSER_INIT

FunctionEnd

;--------------------------------
;Uninstaller Section

Section "un.Uninstall Core Files (required)" SectionCoreUninstall
  SectionIn RO

  ;Stuff to uninstall/remove....
  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\x86\SQLite.Interop.dll"
  Delete "$INSTDIR\x64\SQLite.Interop.dll"
  Delete "$INSTDIR\System.Data.SQLite.Linq.dll"
  Delete "$INSTDIR\System.Data.SQLite.EF6.dll"
  Delete "$INSTDIR\System.Data.SQLite.dll"
  Delete "$INSTDIR\QTemplates.exe"
  Delete "$INSTDIR\QInterfaces.dll"
  Delete "$INSTDIR\Plugins\QTemplates.Example.Plugin.dll"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\EntityFramework.SqlServer.dll"
  Delete "$INSTDIR\EntityFramework.dll"

  ${If} $MultiUser.InstallMode == "CurrentUser"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall (Current User).lnk"
    Delete "$DESKTOP\${PRODUCT_NAME} (Current User).lnk"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME} (Current User).lnk"
  ${Else}
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk"
    Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk"
  ${EndIf}

  RMDir "$SMPROGRAMS\${PRODUCT_NAME}"
  RMDir "$INSTDIR\x86"
  RMDir "$INSTDIR\x64"
  RMDir "$INSTDIR\Plugins"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"

SectionEnd

Section /o "un.Remove Configuration" SectionRemoveConfig

  IfFileExists "${SQLITE_FILE}" 0
    Delete "${SQLITE_FILE}"
  IfFileExists "${CONFIG_DIRECTORY}\*.*" 0
    RMDir "${CONFIG_DIRECTORY}"

SectionEnd

;--------------------------------
;Descriptions for Uninstaller

  ;Assign descriptions to sections
  !insertmacro MUI_UNFUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionCoreUninstall} "The core files required by ${PRODUCT_NAME}."
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionRemoveConfig} "Leave this unchecked if you plan to use ${PRODUCT_NAME} again."
  !insertmacro MUI_UNFUNCTION_DESCRIPTION_END
  
;--------------------------------
;Uninstaller Functions

Function un.onInit

  !insertmacro MULTIUSER_UNINIT

FunctionEnd