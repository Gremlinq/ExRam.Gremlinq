git checkout -b PrepareRelease
$nbgv = nbgv prepare-release --format json | ConvertFrom-Json
git rebase $nbgv.NewBranch.Name $nbgv.CurrentBranch.Name -Xtheirs
git tag $nbgv.NewBranch.Name $nbgv.NewBranch.Name
git branch -d $nbgv.NewBranch.Name