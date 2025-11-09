//day 3

== requirements ==

//TODO
{tul_grown >2: -> day_03_tul}
-> END

==day_03_tul==
# ruth spawn north
noop
# ruth goto field
noop
# ruth plant tulip
noop

# erin spawn east
noop
# erin goto tul_seed
noop

# ruth say
Here to sabotage my tulips some more?
# ruth say
I can't believe your mother let's you go outside without an adult to watch you.
# ruth say
Where is she anyhow?
#ruth say
I saw her car leave at 5am this morning. Is she working this weekend?

# erin say
Yeah. She is always working. She's very, very busy.
# erin say 
... I'm not allowed to go outside alone. 
# erin say
I'm being bad, not my mom.
# erin say
But also, I can't just do nothing all day. I'm bored.

# ruth say
Hm. Well, that much is obvious. //TODO 
# ruth say
I'll talk to your mother. See what I can do.

# erin say
Really? Thank you Mrs. Waylin!

# erin despawn east
noop

# ruth plant tul
noop


# ruth despawn north
noop

# wait
noop

# ruth spawn north
noop
# ruth goto tul
noop


# barbara spawn west
noop
# barbara goto field
noop

# barbara say
Why isn't Bertie here? I thought he loves gardening.

# ruth say
He passed away Barbara.
# ruth say 
It's been a couple months now.
# ruth say 
I'm just putting whatever was left in the flower boxs to use.
# ruth say 
Better than them rotting away.

# barbara say
My condolences. I hadn't heard.

# ruth say
Yes, Erin said you've been busy.
# ruth say
What happened to-

# barbara say
We're not together anymore.

# ruth say
I had thought as much. I'm assuming you have had a difficult time finding someone to look after Erin?

# barbara say
Look, I'm trying my best here-

# ruth say 
That was not an accusation.
# ruth say
I can quite relate to how your daughter must be feeling.
# ruth say
Being alone at home all day has also left me quite "bored", as Erin would put it.
//god I hope reader can tell that it means lonely

# barbara say
...

# ruth say
I wanted to take up gardening to get out of the house anyhow.
# ruth say 
With a community garden right outside of the house, I might as well watch Erin too.

# barbara say
It'd be a load off my shoulders Ruth.
# barbara say
And, I'm sorry. I know I used to come by, but with everything going on I haven't had the time to.

# barbara despawn north
# ruth despawn north
noop

# wait

-> END
