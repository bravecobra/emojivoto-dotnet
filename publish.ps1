$GitVersion = gitversion | ConvertFrom-Json
$Version = $GitVersion.SemVer
docker build --build-arg SemVer=$Version -f ./src/EmojiSvc/Dockerfile -t emojisvc:$Version -t emojisvc:latest .
docker build --build-arg SemVer=$Version -f ./src/EmojiVoting/Dockerfile -t emojivoting:$Version -t emojivoting:latest .
docker build --build-arg SemVer=$Version -f ./src/EmojiUI/Dockerfile -t emojiui:$Version -t emojiui:latest .
docker build --build-arg SemVer=$Version -f ./src/EmojiVoteBot/Dockerfile  -t emojivotebot:$Version -t emojivotebot:latest .
