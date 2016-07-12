function Get-Temperature {
    $t = Get-WmiObject MSAcpi_ThermalZoneTemperature -Namespace "root/wmi"

    foreach ($temp in $t)
    {
        $currentTempKelvin = $temp.CurrentTemperature / 10
        $currentTempCelsius = $currentTempKelvin - 273.15

        $returntemp = @("Temp of agent: " + $temp.InstanceName)
        $returntemp += "temperature"
        $returntemp += "temperatureCelsius"
        $returntemp += $currentTempCelsius.ToString()
        $returntemp | Out-File -FilePath "..\$([System.IO.Path]::GetRandomFileName()).txt" -Encoding utf8
    }
}

while($true) {
    Get-Temperature
    Get-Date
    Sleep 1
}
