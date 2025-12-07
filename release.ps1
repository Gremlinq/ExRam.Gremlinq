$versionIncrement=$args[0]

$nbgvString = if ($versionIncrement -eq $null) { nbgv prepare-release --format json } else { nbgv prepare-release --format json --versionIncrement $versionIncrement }
$nbgv = $nbgvString | ConvertFrom-Json

git checkout $nbgv.NewBranch.Name
git commit --amend --no-edit -S
git rebase $nbgv.NewBranch.Name $nbgv.CurrentBranch.Name -Xtheirs
git tag $nbgv.NewBranch.Name $nbgv.NewBranch.Name
git branch -d $nbgv.NewBranch.Name
