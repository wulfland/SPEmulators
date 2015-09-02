$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".Tests.", ".")
. "$here\$sut"

Describe "init" {
    Context "Visual Studio Version before 2012"{
        Mock Get-Version { return 10 }

        It "throws excetion" {
            { init } | Should Throw "This package requires minimum Visual Studio 2012."
        }
    }

    Context "Visual Studio 2012 Update 0 and 1"{
        Mock Get-Version { return 11 }
        Mock Get-BuildNumber { return 60314}
        Mock Get-Edition { return "Professional" }

        It "throws excetion for edition 'Professional'" {
            { init } | Should Throw "This package requires minimum the Ultimate Edition of Visual Studio 2012 if you have not updated to Update 2."
        }

        Mock Get-Edition { return "Premium" }

        It "throws excetion for edition 'Premium'" {
            { init } | Should Throw "This package requires minimum the Ultimate Edition of Visual Studio 2012 if you have not updated to Update 2."
        }

        Mock Get-Edition { return "Ultimate" }

        It "is valid edition 'Ultimate'" {
            { init } | Should Not Throw
        } 
    }

    Context "Visual Studio 2012 Update 2"{
        Mock Get-Version { return 11 }
        Mock Get-BuildNumber { return 60315}
        Mock Get-Edition { return "Professional" }

        It "throws excetion for edition 'Professional'" {
            { init } | Should Throw "This package requires minimum the Premium Edition of Visual Studio 2012 Update 2."
        }

        Mock Get-Edition { return "Ultimate" }

        It "is valid edition 'Ultimate'" {
            { init } | Should Not Throw
        }

        Mock Get-Edition { return "Premium" }

        It "is valid edition 'Premium'" {
            { init } | Should Not Throw
        }
    }

    Context "Visual Studio 2013"{
        Mock Get-Version { return 12 }
        Mock Get-Edition { return "Premium" }

        It "is valid edition 'Premium'" {
            { init } | Should Not Throw
        }

        Mock Get-Edition { return "Ultimate" }

        It "is valid edition 'Ultimate'" {
            { init } | Should Not Throw
        }

        Mock Get-Edition { return "Professional" }

        It "throws excetion for edition 'Professional'" {
            { init } | Should Throw "This package requires minimum the Premium Edition of Visual Studio 2013."
        }
    }

    Context "Visual Studio 2015"{
        Mock Get-Version { return 14 }
        Mock Get-Edition { return "Professional" }

        It "throws excetion for edition 'Professional'" {
            { init } | Should Throw "This package requires minimum the Enterprise Edition of Visual Studio 2015."
        }

        Mock Get-Edition { return "Enterprise" }

        It "is valid edition 'Enterprise'" {
            { init } | Should Not Throw
        }
    }
}
