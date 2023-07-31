MEAN-MEDIAN-MODE GRAPH

stated purpose: 
 - Interactable bar graph to help students calculate mean/median/mode.
 - Students can click/drag specific bars and move them around, changing their order

TODO
 H. Fix reset button
 J. Bars move dynamically, not when you let go 'The other bars should reorder as the user is moving their selected bar so the graph doesn’t ever rest with missing or overlapping bars on any given spot.'
 5. IMoveable interface

LOW-PRI
 I. Fix in-headset interaction issue
 Q. Only right hand is supported. Support left hand.
 
EXTRA
 1. display mean/median/mode 
 2. You can change the values of the bars and see the changes reflected

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
 






