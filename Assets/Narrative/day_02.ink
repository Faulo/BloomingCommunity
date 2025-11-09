//day 2
VAR tul_grown = 0
VAR dan_grown = 0

== requirements ==
{tul_grown >0: -> day_02_tul}
{dan_grown >0: -> day_02_dan}
-> END

==day_02_tul==
# ruth spawn north
noop
# ruth goto tul_grown
noop
# ruth say
Oh. I didn't know tulips could grow that fast.
//if you can code a pause
# ruth say
In the end Bertie did get me into his silly little hobby after all. 
#ruth say
Sly old fox.

# erin spawn west
# barbara spawn west
noop
# erin say
Mom, can we look at the garden thingy today?
# erin goto field
noop
# erin say
I wanna see my flower!
# barbara goto tul_grown
noop


# barbara say
Those usually don't pop up overnight hon. Honestly Erin, I still need to-
# barbara say
Oh! Mrs. Weylin. I hadn't seen you around much.

# ruth say
...lovely to see you too, Barbara. //adjust for tone?
# ruth say
I'm doing fine, thank you for asking.
//thing about how barbara used to make an effort to see ruth but hasn't lately

#erin goto tul_grown
noop
# erin say
That's a pretty flower!
# erin say
It's not the daintylion I planted yesterday though.

# ruth say
You put weeds in my flower patch?
# ruth say
Count yourself lucky those weeds didn't fester and my tulips survived.
# ruth say
Barbara, talk some sense into your child!

# barbara say
It's not "your garden Ruth", it's a community garden.
# barbara say
And I'm not yelling at my kid for that. //needs something

# ruth say
You take no accountability for you child! You can't even be bothered to supervise her.
# ruth say
No woder she's running around mucking up the hard work of other people.
# ruth harvest tul_grown
noop

# barbara say
I watch my kid just fine. 

# ruth despawn north
noop


#barbara say
Can mind her own damn buissness.
# barbara say
Let's head in Erin.

# erin say
But I want to plant something! Before Mrs. Waylin takes control of everything-

# barbara say
Tomorrow Erin, I still have to get the car fixed today.
# barbara say
Dinner's in the fridge, I'll be home as soon as I can.

# erin say
But-

# barbara say
I'm already late, no buts.

# barbara despawn east
# erin despawn east
noop 

# barbara spawn east
noop
# barbara despawn west
-> END

==day_02_dan==
# ruth spawn north
noop
#ruth goto dan_grown
noop
# ruth say
Ugh! Like I said, overrun.

//Erin runs ahead
# erin spawn west
# barbara spawn west
noop
#erin goto dan_grown
noop
# erin say
Mom look! The flower I planted grew up!
#barbara goto field
# erin say
I love these fluffy guys!
# barbara say
Erin, I really don't have time for this.
# erin harvest dan_grown

# ruth say
Erin dear, that is a weed. Nobody plants them, they crawl in on their own.
#ruth say
Stop spreading the seeds around, you're making this so called "community project" more hopeless than it already was. //needs something
#ruth say
Not that I give those tulips much of a fighting chance anyhow. Bertie was the one with the green thumb, not me.

# erin say
No Mrs. Wylin, I planted this myself! I'ts my favorite flower, I think.
#erin say
Where is Mr. Wylin? He'd love this.

# ruth day
Yes, he would have loved this.

# barbara say
Erin, when exactly did you plant a dandelion? You were asleep when I got home last night.

# erin say
Ummmm-

# barbara say
Erin Garcia Mendez. You know better than to leave the house without someone to watch you!

# erin say

//TODO



->END





