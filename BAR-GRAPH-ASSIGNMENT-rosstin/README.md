MEAN-MEDIAN-MODE GRAPH

stated purpose: 
 - Interactable bar graph to help students calculate mean/median/mode.
 - Students can click/drag specific bars and move them around, changing their order

TODO
 z11. Pack up the APK
 z12. Zip the project
 z13. Send it

KNOWN BUGS
 1. you can only select each bar once in-headset. Hitting "reset" fixes this.
 2. Only right hand is supported. Support left hand.
 3. Bar behaves badly if you move the selected bar faster than the animation speed of the other bars - improving and refactoring their states will probably fix this
 4. Output text in-headset displays grip text instead of MMM
 5. In-headset button assignment is sloppy - grip moves objects but trigger activates reset button

FUTURE WORK
 R.  Replace updates with coroutines on Bar object to improve perf
 S.  Sorts can be improved
 T.  Unit tests for calculations in util class
 X.  Should move interaction stuff out of Graph.cs
 z7. Add median/mode calcs

EXTRA
 e2. You can change the values of the bars and see the changes reflected in the meanmedianmode

DONE
 1. Click and drag bar object
 2. Bar object is free in horiz, locked in vert
 3. Bars instantly self-order (todo smoothly self-order)
   3a. Actually reorder bars now
   3b. For starters, we can make the bar position "grainy" - linked directly to its X
   3c. Calculate X by using the graph's span assuming that the right edge of the graph is, say, 60 index units
   3d. Bars are always ordered by index - when you let go of a bar, check if it's in the correct place still
 4. Bars move smoothly, not instantly
 6. Bar includes index and value numbers 
 7. Bars display given data (see pdf)
 A. Gripup event unselects bar 
 B. Fix bar in Y
 D. Bar knows her new index based on X
 G. Bar's index is represented by graph index, not Unity X Pos
 H. Fix reset button
 J. The other bars should reorder AS THE USER IS MOVING THEIR SELECTED BAR 
   Ja. An issue with index sorting causes no gap for the currently moving bar
   Jb. Bars moving slowly probably because they're getting triggered too often
   Jc. It was because of the grainy movement - there was often an equality. Changing to float fixes this.
 K. the graph doesn’t ever rest with missing or OVERLAPPING bars on any given spot
   Ka. done - selected bar pops forward enuff to prevent them touching
 L. the selected bar could move forward smoothly when selected to avoid overlaps - i can do this by modifying local mesh
 e1. display mean/median/mode 
 z1. Occasional glitch with dynamic positioning - investigate
 z2. increasing the movement speed mostly gets rid of this, so its an issue of something relating to starting and stopping movement perhaps, and states locking while things are in motion. will simply increase the speed for now and investigate later
 z10. Make a video

 z5. Add a "point" object so that we can pull out more requirements for the IMoveable interface
 5.  IMoveable interface
 N.  The IMoveable interface should "hide" the Bar class from the graph better such that the Graph doesn't know so much about Bar
 M.  Cleanup
 z4. Improve IMoveable interface by adding another IMoveable object - the point








