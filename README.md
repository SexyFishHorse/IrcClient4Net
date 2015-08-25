# IrcClient4Net
*Stupid and super simple IRC client for the .net Framework*

The idea behind this repository is to provide a stupid and simple library that allows you to establish a connection to an IRC server without all sorts of strange features or odd logic that you may not like, want or need.

You get all the methods you need to connect and interact with the server along with methods that allows you to receive messages in a parsed format. You will have to do the rest yourself.

This library is specifically built for use with the Twitch and Hitbox live stream chats in mind so some features may be missing for now. Create an issue or submit a pull request and I'll look into it as soon as possible :)

I will do my best to keep twitch and hitbox logic in their respective classes so you don't have to mess with that if you don't need it. I might even move it into its own repository eventually depending on how large that functionality or the basic framework evolves.

# THIS PROJECT IS IN A VERY EARLY STAGE OF DEVELOPMENT
The base ```IrcClient``` is very feature lacking at the moment. It does however already provide basic functionality to connect and interact with an IRC server and it can return the messages in either the raw format or in a parsed format (however I haven't gotten around to make a disconnect method yet. This is only a few hours old)

A few commands (```PASS```, ```USER```, ```NICK```, ```JOIN```, ```PART```, ```PRIVMSG``` and ```CAP REQ```) have been added to the ```IrcCommandFactory``` class for ease of use, but you may have to create your own raw messages for now until more commands are supported.

The ```TwitchIrcClient``` is built as a wrapper around the ```IrcClient``` and provides an easier way to connect directly to the twitch irc for a specific channel. Enter your twitch username and your [tmi token](https://twitchapps.com/tmi/) in the App.config file and call ```.Connect``` on the ```TwitchIrcClient```.

# For contributors
Please maintain the same structure and coding style as in the existing code for improved readability.
