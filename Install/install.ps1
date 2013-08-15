param($installPath, $toolsPath, $package, $project)

$projectIsVB = $false
try {
    $item = $project.ProjectItems.Item("global.asax").ProjectItems.Item("global.asax.cs")
} catch {
    $item = $project.ProjectItems.Item("global.asax").ProjectItems.Item("global.asax.vb")
    $projectIsVB = $true
}

if ($projectIsVB) {
    $class = $item.FileCodeModel.CodeElements | where-object {$_.Kind -eq 1}
} else {
    $namespace = $item.FileCodeModel.CodeElements | where-object {$_.Kind -eq 5}
    $class = $namespace.Children | where-object {$_.Kind -eq 1}
}

$method = $class.Children | where-object {$_.Name -eq "Application_Start"}
if (!$method)
{
    [system.windows.forms.messagebox]::show("methods is null")
}

$edit = $method.StartPoint.CreateEditPoint();
$edit.LineDown()
$edit.CharRight(1)
$edit.Insert([Environment]::NewLine)
if ($projectIsVB) {
    $edit.Insert("      ModelBinders.Binders.DefaultBinder = new PerpetuumSoft.Knockout.KnockoutModelBinder()")
    $edit.Insert([Environment]::Newline)
} else {
    $edit.Insert("      ModelBinders.Binders.DefaultBinder = new PerpetuumSoft.Knockout.KnockoutModelBinder();")
}
