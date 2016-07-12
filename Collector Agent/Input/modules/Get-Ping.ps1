function Get-Ping {
    param(
        $ComputerName
    )

    $t = $(Test-Connection -ComputerName $ComputerName -Count 1 | select *)

    $returntemp = @("Ping to " + $t.Address)
    $returntemp += "ping"
    $returntemp += "rtt"
    $returntemp += $t.ResponseTime
    $returntemp += "ttl"
    $returntemp += $t.ResponseTimeToLive
    $returntemp += "icmp type"
    $returntemp += $t.StatusCode
    $returntemp | Out-File -FilePath "..\$([System.IO.Path]::GetRandomFileName()).txt" -Encoding utf8
}

while($true) {
    Get-Ping -ComputerName "8.8.8.8"
    Get-Ping -ComputerName "8.8.4.4"
    Get-Ping -ComputerName "google.de"
    Get-Ping -ComputerName "google.com"
    Get-Ping -ComputerName "plan.de"
    Get-Ping -ComputerName "compilenix.org"
    Get-Ping -ComputerName "twitter.com"
    Get-Ping -ComputerName "github.com"
    Get-Ping -ComputerName "youtube.com"
    Get-Ping -ComputerName "blog.fefe.de"
    Get-Ping -ComputerName "heise.de"
    Get-Ping -ComputerName "schneier.com"
    Get-Ping -ComputerName "gentoo.org"
    Get-Ping -ComputerName "ietf.org"
    Get-Ping -ComputerName "wikipedia.org"
    Get-Ping -ComputerName "yahoo.com"
    Get-Ping -ComputerName "bing.com"
    Get-Ping -ComputerName "yandex.ru"
    Get-Ping -ComputerName "reddit.com"
    Get-Ping -ComputerName "stackoverflow.com"
    Get-Ping -ComputerName "pastebin.com"
    Get-Date
    sleep 1
}
