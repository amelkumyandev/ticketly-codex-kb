param(
    [string]$Path = "TOKEN_BURN.md",
    [int]$Task = 0
)

if (-not (Test-Path $Path)) {
    Write-Error "TOKEN_BURN.md was not found at path: $Path"
    exit 1
}

$content = Get-Content $Path -Raw

function Get-SectionContent {
    param(
        [string]$Text,
        [string]$SectionPattern
    )

    $matches = [regex]::Matches($Text, $SectionPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    $result = ""

    foreach ($match in $matches) {
        $result += $match.Groups[1].Value
        $result += "`n"
    }

    return $result
}

if ($Task -gt 0) {
    $inputPattern = "(?s)## Task $Task Input\s*(.*?)(?=## Task $Task Output Summary|\z)"
    $outputPattern = "(?s)## Task $Task Output Summary\s*(.*?)(?=## Task \d+ Input|\z)"
}
else {
    $inputPattern = '(?s)## Task \d+ Input\s*(.*?)(?=## Task \d+ Output Summary|\z)'
    $outputPattern = '(?s)## Task \d+ Output Summary\s*(.*?)(?=## Task \d+ Input|\z)'
}

$inputText = Get-SectionContent -Text $content -SectionPattern $inputPattern
$outputText = Get-SectionContent -Text $content -SectionPattern $outputPattern

$inputChars = $inputText.Length
$outputChars = $outputText.Length
$totalChars = $inputChars + $outputChars

$inputTokens = [math]::Ceiling($inputChars / 4)
$outputTokens = [math]::Ceiling($outputChars / 4)
$totalTokens = $inputTokens + $outputTokens

Write-Host "Token-burn estimate"
Write-Host "==================="
Write-Host "Method: estimated local usage"
Write-Host "Formula: estimated tokens = characters / 4"
Write-Host "Scope: $(if ($Task -gt 0) { "Task $Task" } else { "All task sections" })"
Write-Host ""
Write-Host "Input characters:  $inputChars"
Write-Host "Output characters: $outputChars"
Write-Host "Total characters:  $totalChars"
Write-Host ""
Write-Host "Input tokens:      $inputTokens"
Write-Host "Output tokens:     $outputTokens"
Write-Host "Total tokens:      $totalTokens"
Write-Host ""
Write-Host "Note: This is not exact billing usage. It is a repeatable local estimate for experiment comparison."
