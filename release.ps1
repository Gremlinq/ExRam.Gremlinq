$versionIncrement=$args[0]
$current = $(git branch --show-current)

git checkout -b PrepareRelease
$nbgvString = if ($versionIncrement -eq $null) { nbgv prepare-release --format json } else { nbgv prepare-release --format json --versionIncrement $versionIncrement }
$nbgv = $nbgvString | ConvertFrom-Json

git rebase -f $current $nbgv.NewBranch.Name
git rebase $nbgv.NewBranch.Name $nbgv.CurrentBranch.Name -Xtheirs
git tag $nbgv.NewBranch.Name $nbgv.NewBranch.Name
git branch -d $nbgv.NewBranch.Name
git push --set-upstream origin PrepareRelease
git checkout $current
git merge PrepareRelease
git branch -d PrepareRelease