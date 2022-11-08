$GitVersion = gitversion | ConvertFrom-Json
$Version = $GitVersion.SemVer

docker tag emojisvc:latest localhost:5000/emojisvc:$Version
docker push localhost:5000/emojisvc:$Version

docker tag emojivoting:latest localhost:5000/emojivoting:$Version
docker push localhost:5000/emojivoting:$Version

docker tag emojiui:latest localhost:5000/emojiui:$Version
docker push localhost:5000/emojiui:$Version

docker tag emojivotebot:latest localhost:5000/emojivotebot:$Version
docker push localhost:5000/emojivotebot:$Version
