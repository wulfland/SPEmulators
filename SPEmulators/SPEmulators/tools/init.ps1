if ($DTE.Version -lt "11")
{
	throw "This package requires minimum Visual Studio 2012"
}
if (($DTE.Version -eq "11.0") -and ($DTE.Edition -ne "Ultimate"))
{
	throw "This package requires the Ultimate Edition of Visual Studio 2012"
}
if ($DTE.Version -eq "12.0")
{
	if (($DTE.Edition -ne "Premium") -and ($DTE.Edition -ne "Ultimate"))
	{
		throw "This package requires minimum the Premium Edition of Visual Studio 2013"
	}
}
