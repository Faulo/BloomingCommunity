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
# ruth goto field
noop
# ruth say
Oh. I didn't know tulips could grow that fast.
//if you can code a pause
# ruth say
In the end Bertie did get me into his silly little hobby after all. Sly old fox.

# erin spawn west
# barbara spawn west
noop
# erin say
Mom, can we look at the garden thingy today?
# erin say
I wanna see my flower!
# erin goto field
# barbara goto field
noop

# barbara say
Those usually don't pop up overnight hon. Honestly Erin, I still need to-
# barbara say
Oh! Mrs. Weylin. I hadn't seen you around much.

# ruth say
...lovely to see you too, Barbara. //adjust for tone?
# ruth say
I'm doing fine, thank you for asking.

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
something mean
//rant about accountability or something
# ruth despawn north

# barbara say
//needs something
I watch my kid just fine. Can mind her own damn buissness.
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

//put conflict in dandy route
# barbara say
Erin, when exactly did you plant a dandelion? You were asleep when I got home last night.

# erin say
Ummmm-

# barbara say
Erin Garcia Mendez. You know better than to leave the house without someone to watch you!

# erin say
I don
//TODO

-> END

==day_02_dan==
# ruth say
Ugh! Like I said, overrun.

//Erin runs ahead
# erin say
Mom look! The flower I planted grew up!
# erin say
I love these fluffy guys!

# ruth say
Erin dear, that is a weed. Stop spreading the seeds around, you're making this so called community project more hopeless than it already was. //needs something

// Mom catches up
# barbara say
Erin, I really don't have time for this.

# erin say
No Mrs. Wylin, I planted this myself! I'ts my favorite flower, I think.



->END





