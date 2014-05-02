if (($DTE.Edition -ne "Ultimate") -or ($DTE.Version -lt "11"))
{
	throw "This package requires minimum Visual Studio 2012 Ultimate"
}
