# Emby.Plugin.MatrixNotification

Move to: https://codeberg.org/bjoerns1983/Emby.Plugin.MatrixNotification

Emby Server Plugin for sending notifications to a Matrix Room.
For Emby Server Version up and including 4.7 use Version 1.0 of the Plugin.
For Emby Server Version 4.8.20 and up use Version 1.1 of the Plugin.

## Read first
The Plugin does not support End-to-End Encyrption, simply because there is no SDK for it. Of course your traffic will be encrypted using TLS.
It should work with all Matrix Servers which implement the Client-Server-API.
I have tested it with an account at https://matrix.org
The Plugin will forward everything it gets from the Emby Standard Notification Framework, not more. Yeah but it is limited but at least a stable option over the past years.

## Install
1. Install the Plugin by downloading the DLL (or build it yourself using VS2017/VS2019) and putting it into your Emby Plugin folder
2. Restart your Emby Server
3. Grab yourself the Element Messenger and curl (of something similier to talk with the API)
4. Your server needs a user account on the matrix instance to use, so register on for it
5. Start Element with your own account and create a room, deactive encryption (important), i made mine invite only.
6. Invite your Emby Server to the room
7. Head to the room settings you will find the internal Room ID, something like: !klblablabla:matrix.org an note it

Now it is time to start playing with curl to get Access Token of your Emby user and join the room
Mind changing the TLD etc accoring to your needs.

8. Start with getting your Access Token
curl -XPOST -d '{"type":"m.login.password", "user":"EMBYSERVERUSER", "password":"EMBYSERVERPASSWORD"}' "https://matrix.org/_matrix/client/r0/login"

9. Join the room (the ! in teh room ID must be exchanged with ah %21 ): 
curl -XPOST -d '{}' "https://matrix.org/_matrix/client/r0/rooms/%21klblablabla:matrix.org/join?access_token=ACCESSTOKEN"

10. Test the connection
curl -XPOST -d '{"msgtype":"m.text", "body":"hello"}' "https://matrix.org/_matrix/client/r0/rooms/%21klblablabla:matrix.org/send/m.room.message?access_token=ACCESSTOKEN"

11. Go to the Plugin settings page and fill in the Access Token, Room ID (like you copied it from Element aka with the ! at front), and the Server TLD (dont forget the / at the back)
12. Send a Test Notification
13. Activate the Matrix Notifications Plugin in the desired Server notifications


 
