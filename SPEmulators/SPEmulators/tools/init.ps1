# Fakes was introduced in VS2012
if ($DTE.Version -lt "11"){
	throw "This package requires minimum Visual Studio 2012."
}

$BuildNumber = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($DTE.FullName).ProductBuildPart
if ($BuildNumber -ge "60315"){
	# Since VS2012 it is available in the PremiumEdition
	if (-not ($DTE.Edition -eq "Premium") -or ($DTE.Edition -eq "Ultimate")){
		throw "This package requires minimum the Premium Edition of Visual Studio 2012 Update 2."
	}
}else{
	if ($DTE.Edition -ne "Ultimate"){
		throw "This package requires minimum the Ultimate Edition of Visual Studio 2012 if you have not updated to Update 2."
	}
}