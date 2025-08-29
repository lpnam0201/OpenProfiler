set OpenProfilerDllBinFolder=..\OpenProfiler\bin\Debug\net8.0
ilrepack /out:..\Tests\OpenProfilerDll\OpenProfiler.dll ^
	%OpenProfilerDllBinFolder%\OpenProfiler.dll ^
	%OpenProfilerDllBinFolder%\Microsoft.CodeAnalysis.dll ^
	%OpenProfilerDllBinFolder%\Microsoft.CodeAnalysis.CSharp.dll ^
	%OpenProfilerDllBinFolder%\System.Collections.Immutable.dll ^
	%OpenProfilerDllBinFolder%\System.Reflection.Metadata.dll