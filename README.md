# Budapest-Metro

This is my 2nd attempt of replicating the subway network of Budapest within Unity.
The previous project isn't public.

Due to a lack of precise data:

- tracks don't fully resemble their real-world curvatures and elevation
- the timetable is only precise to the minute, causing some punctuality errors

At the moment, only the M4 line has been built (including the carriage house).
It's objectively the best metro in the world, so I had to build it first :3

Trains get dispatched and recalled automatically.
You can select when and where to start the game.

> [!TIP]
> Press `[TAB]` to open the in-game menu.

> [!IMPORTANT]
> The time scale input is broken because Unity decided that it doesn't want to be focused.
> I really don't know what I've done wrong...

# Assets

Sounds were recorded and edited by me.

For now, I've imitated the announcers' voices.

I created the models; vehicles are based on [András Vígh's drawings](http://vigimodell.hu/kep/jelleg/)

Yeah, they aren't pretty, but they somewhat resemble the vehicles :3

There's quite a bit of public information about metro line M4 on its [official website](https://www.metro4.hu/en)

# Future Plans

I plan on reaching out to BKK (Budapest Transportation Center) for permission to use the on-board announcements
and to hopefully gather more information to match real-world infrastructure.

# Technical Info

## SplineMesh

I've imported and iterated on [SplineMesh](https://assetstore.unity.com/packages/tools/modeling/splinemesh-104989)
to create tracks, and have the meshes automatically follow the given spline.

You can read my optimizations [here](https://github.com/Axwabo/SpaceTransit#splinemesh)

## Track Segments

I've combined my previous iteration of segment-based splines and the remapping method in
[SpaceTransit](https://github.com/Axwabo/SpaceTransit) to create easy-to-edit and less error-prone
turnout mechanisms.

Non-destructive track splitting and an editor menu item make it easier to connect "two tracks."
The TrackSplitter component creates persistent "sub-segments" based on the given track.

Since the project is spline-based, there's no "go that way" like in the real world, therefore we have to
account for branching (forwards) and joining (backwards) turnouts. The menu item detects this automatically.

The editor has switch groups, which, when selected, show handles that let you drag the turnout
towards the branch to switch to. I'll improve it later as the snapping is a tiny bit clunky.

In SpaceTransit, I have to manually set which tube should be connected to which,
which can be confusing as the direction matters.
Plus, after cloning remappers, it's easy to forget to assign a tube.

In my previous metro project, I created a complicated system for wheels to detect
that they've reached a different spline.

## UI Rendering



## Tiling Shader

Creating a 