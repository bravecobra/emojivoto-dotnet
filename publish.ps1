$GitVersion = gitversion | ConvertFrom-Json
$Version = $GitVersion.SemVer
docker build -f ./src/EmojiSvc/Dockerfile -t emojisvc:$Version .
docker build -f ./src/EmojiVoting/Dockerfile -t emojivoting:$Version .
docker build -f ./src/EmojiUI/Dockerfile -t emojiui:$Version .
docker build -f ./src/EmojiVoteBot/Dockerfile  -t emojivotebot:$Version .
