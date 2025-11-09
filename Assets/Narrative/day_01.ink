VAR tul_grown = 0
VAR dan_grown = 0
-> day_01

== requirements ==
-> day_01
-> END

=== day_01 ===

# erin spawn west
# barbara spawn west
# erin say
WHAT! is THAT!!

# erin goto field
# barbara goto field

# barbara say
Erin, stop pulling on my arm. 
# barbara say
That's where I'm carrying our groceries, aka all your precious snacks! //cocoa puffs, snacks, etc

# erin say
Mom look! There's a little garden!

# barbara say
Cute. But, the little garden will still be there tomorrow. 
# barbara say
I need to catch a couple of Zs before my night shift Hon. Come on.

# erin say
Awww :c But then I have nothing to dooooo. It's booorriiinngggg.

# barbara say
Kid, there ain't two of me. Sit tight, we'll take a look tomorrow after I'm done with work.
//setup, have her use this line multiple times

# erin say
\*grumble grumble\*
//"when I'm off work." Which happens when? 'Off work' isn't a real time!
//It's been a bazillion years
# barbara goto east
# erin goto east
# barbara despawn
# erin despawn

# ruth spawn north
# ruth goto field
# ruth say
...they made a new patch of dirt. Right next to my house.
# ruth say
Lovely. What do I even pay property taxes for when the municipality uses it on ugly, useless nonsence like this.
# ruth say
They should have put down some flowers immedeatly. This will be overrun with weeds and god knows what before the week is up.
# ruth goto north
# ruth despawn
# ruth spawn north
# ruth goto field
# ruth say
...
Bertie had left some tulip bulbs in the window boxes.
# ruth plant tul
# ruth goto north
# ruth despawn
//option erin and ruth interact, ruth mad about weeds
// on day 2 ruth mad at mom for not looking after child?


//later that day?
# erin spawn east
# erin goto field
# erin say
I can leave the house a little. I'm 8! That's almost 10!
# erin plant dan
# erin say
There c: puffy flower. Awesome.
# erin goto east
# erin despawn

//day end
-Line End
-> END