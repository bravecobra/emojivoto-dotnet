docker build -f ./src/EmojiSvc/Dockerfile -t emojisvc:latest .
docker build -f ./src/EmojiVoting/Dockerfile -t emojivoting:latest .
docker build -f ./src/EmojiUI/Dockerfile -t emojiui:latest .
docker build -f ./src/EmojiVoteBot/Dockerfile  -t emojivotebot:latest .
