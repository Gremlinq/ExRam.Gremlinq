$current = $(git branch --show-current)
git checkout -b PrepareRelease
$nbgv = nbgv prepare-release --format json | ConvertFrom-Json
git rebase $current $nbgv.NewBranch.Name
git rebase $nbgv.NewBranch.Name $nbgv.CurrentBranch.Name -Xtheirs
git tag $nbgv.NewBranch.Name $nbgv.NewBranch.Name
git branch -d $nbgv.NewBranch.Name
git push
git checkout $current
git merge PrepareRelease
git branch -d PrepareRelease