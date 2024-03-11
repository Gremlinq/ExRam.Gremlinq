$current = $(git branch --show-current)
$nbgv = nbgv prepare-release --format json | ConvertFrom-Json
git rebase $nbgv.NewBranch.Name $nbgv.CurrentBranch.Name -Xtheirs
git tag $nbgv.NewBranch.Name $nbgv.NewBranch.Name
git branch -d $nbgv.NewBranch.Name
git checkout -b PrepareRelease
git push
git checkout $current
git branch -d PrepareRelease