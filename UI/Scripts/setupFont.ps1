$solPath = $args[0]

if (Test-Path "env:windir") {
	$fontsPath = "$solPath\Assets\Fonts"
	New-Item -ItemType Directory -Force -Path $fontsPath

	$segoeUiPath = "$env:windir\Fonts\seguiemj.ttf"
    if([System.IO.File]::Exists($segoeUiPath)) {
		Write-Output "Copying font 'Segoe UI Emoji' (seguiemj.ttf)..."
		Copy-Item -Force -Recurse -Path $segoeUiPath -Destination $fontsPath\seguiemj.ttf
		Write-Output "'Segoe UI Emoji' copied."
    }
    else {
		Write-Error "'Segoe UI Emoji' not found: $segoeUiPath"
		exit 1
	}

    $segoeMDL2AssestsPath = "$env:windir\Fonts\segmdl2.ttf"
	if([System.IO.File]::Exists($segoeMDL2AssestsPath)) {
		Write-Output "Copying font 'Segoe MDL2 Assets' (segmdl2.ttf)..."
		Copy-Item -Force -Recurse -Path $segoeMDL2AssestsPath -Destination $fontsPath\segmdl2.ttf
        Write-Output "'Segoe MDL2 Assets' copied."
	}
	else {
		Write-Error "'Segoe UI Emoji' not found: $segoeUiPath"
		exit 1
	}
    exit 0
}
Write-Output "Skipping font import. No 'windir'"
