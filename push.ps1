$GitVersion = gitversion | ConvertFrom-Json
$Version = $GitVersion.SemVer

docker tag emojisvc:latest localhost:5000/bravecobra/emojisvc:$Version
docker push localhost:5000/bravecobra/emojisvc:$Version
docker tag emojisvc:latest localhost:5000/bravecobra/emojisvc:latest
docker push localhost:5000/bravecobra/emojisvc:latest

docker tag emojivoting:latest localhost:5000/bravecobra/emojivoting:$Version
docker push localhost:5000/bravecobra/emojivoting:$Version
docker tag emojivoting:latest localhost:5000/bravecobra/emojivoting:latest
docker push localhost:5000/bravecobra/emojivoting:latest

docker tag emojiui:latest localhost:5000/bravecobra/emojiui:$Version
docker push localhost:5000/bravecobra/emojiui:$Version
docker tag emojiui:latest localhost:5000/bravecobra/emojiui:latest
docker push localhost:5000/bravecobra/emojiui:latest

docker tag emojivotebot:latest localhost:5000/bravecobra/emojivotebot:$Version
docker push localhost:5000/bravecobra/emojivotebot:$Version
docker tag emojivotebot:latest localhost:5000/bravecobra/emojivotebot:latest
docker push localhost:5000/bravecobra/emojivotebot:latest
