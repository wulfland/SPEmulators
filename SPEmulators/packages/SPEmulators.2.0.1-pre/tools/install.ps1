param($installPath, $toolsPath, $package, $project) 

function Set-FakesFileBuildAction($project, $fakesFileName)
{
	$fakesFolder = $project.ProjectItems.Item("Fakes")
	$fakesFile = $fakesFolder.ProjectItems.Item($fakesFileName)
	$itemTypeProperty = $fakesFile.Properties.Item("ItemType")
	$itemTypeProperty.Value = "Fakes";
}

Set-FakesFileBuildAction $project "System.fakes"
Set-FakesFileBuildAction $project "System.Web.fakes"
Set-FakesFileBuildAction $project "Microsoft.SharePoint.fakes"