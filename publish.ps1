docker build -f ./EmojiSvc/Dockerfile -t emojisvc:latest .
docker tag emojisvc:latest localhost:5000/emojisvc:latest
docker push localhost:5000/emojisvc:latest

docker build -f ./EmojiVoting/Dockerfile -t emojivoting:latest .
docker tag emojivoting:latest localhost:5000/emojivoting:latest
docker push localhost:5000/emojivoting:latest

docker build -f ./EmojiUI/Dockerfile -t emojiui:latest .
docker tag emojiui:latest localhost:5000/emojiui:latest
docker push localhost:5000/emojiui:latest

docker build -f ./EmojiVoteBot/Dockerfile  -t emojivotebot:latest .
docker tag emojivotebot:latest localhost:5000/emojivotebot:latest
docker push localhost:5000/emojivotebot:latest

