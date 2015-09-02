function Get-Version
{
    return [int]$DTE.Version
}

function Get-BuildNumber
{
    return [System.Diagnostics.FileVersionInfo]::GetVersionInfo($DTE.FullName).ProductBuildPart
}

function Get-Edition
{
    return $DTE.Edition
}
   

function init
{   
    $Version = Get-Version
    $Edition = Get-Edition

    # Fakes was introduced in VS2012
    if ($Version -lt 11)
    {
	    throw "This package requires minimum Visual Studio 2012."
    }

    # Since VS2012 it is available in the Premium Edition
    if ($Version -eq 11)
    {
        $BuildNumber = Get-BuildNumber
        if ($BuildNumber -ge 60315)
        {
	
	        if (-not (($Edition -eq "Premium") -or ($Edition -eq "Ultimate")))
            {
		        throw "This package requires minimum the Premium Edition of Visual Studio 2012 Update 2."
	        }
        }
        else
        {
	        if ($Edition -ne "Ultimate")
            {
		        throw "This package requires minimum the Ultimate Edition of Visual Studio 2012 if you have not updated to Update 2."
	        }
        }
    }

    # In VS 2013 Fakes is supported in Premium and Ultimate
    if ($Version -eq 12)
    {
        if (-not (($Edition -eq "Premium") -or ($Edition -eq "Ultimate")))
        {
		    throw "This package requires minimum the Premium Edition of Visual Studio 2013."
	    }
    }

    # In VS2015 Fakes is supported in Enterprise and Premium 
    if ($Version -ge 14)
    {
        if (-not ($Edition -eq "Enterprise"))
        {
		    throw "This package requires minimum the Enterprise Edition of Visual Studio 2015."
	    }
    }
}

# Comment this line out to test the script using pester (see https://github.com/pester/Pester)
init
