if exist "Release" (
    rmdir /s /q "Release"
)
mkdir "Release"

dotnet publish ..\OpenProfiler.GUI\OpenProfiler.GUI.csproj -o "Release\GUI" --self-contained true
dotnet build ..\OpenProfiler\OpenProfiler.csproj --configuration Release

set OpenProfilerDllBinFolder=..\OpenProfiler\bin\Release\net8.0
ilrepack /out:Release\Dll\OpenProfiler.dll ^
	%OpenProfilerDllBinFolder%\OpenProfiler.dll ^
	%OpenProfilerDllBinFolder%\Microsoft.CodeAnalysis.dll ^
	%OpenProfilerDllBinFolder%\Microsoft.CodeAnalysis.CSharp.dll ^
	%OpenProfilerDllBinFolder%\System.Collections.Immutable.dll ^
	%OpenProfilerDllBinFolder%\System.Reflection.Metadata.dll

del Release.zip
7z.exe a -tzip -mx=9 Release.zip files_to_compress .\Release\GUI .\Release\Dll
rmdir /s /q "Release"