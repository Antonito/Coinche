# !/usr/bin/sh

CLIENT_PATH="../Client/bin/Debug/Client.exe"

Player1()
{
	tmux send-keys "echo 'Player 1': $1" Enter
	GAME="0\n80\n0\n0\n"
	tmux send-keys  "(echo -ne '$1\nPlayer1\ntoto\nHi !\n/ready\n$GAME'; cat) | mono $CLIENT_PATH" Enter
}

Player2()
{
	tmux send-keys "echo 'Player 2': $1" Enter
	GAME2='0\n90\n0\n0\n'
	tmux send-keys  "(echo -ne '$1\nPlayer2\ntoto\nHi !\n/ready\n$GAME2'; cat) | mono $CLIENT_PATH" Enter
}

Player3()
{
	tmux send-keys "echo 'Player 3': $1" Enter
	GAME3='0\n100\n0\n0\n'
	tmux send-keys  "(echo -ne '$1\nPlayer3\ntoto\nHi !\n/ready\n$GAME3'; cat) | mono $CLIENT_PATH" Enter
}

Player4()
{
	tmux send-keys "echo 'Player 4': $1" Enter
	GAME4='0\n110\n0\n0\n'
	tmux send-keys  "(echo -ne '$1\nPlayer4\ntoto\nHi !\n/ready\n$GAME4'; cat) | mono $CLIENT_PATH" Enter
}

tmux new -d -s unit_test
tmux rename-window server

echo "Addr:"
read ADDR

# Run clients
Player1 "$ADDR"

tmux split-window -h
Player2 "$ADDR"

tmux split-window -v
Player3 "$ADDR"

tmux select-pane -t 0
tmux split-window -v
Player4 "$ADDR"

tmux attach-session -t unit_test
