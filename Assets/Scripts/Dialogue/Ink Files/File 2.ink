INCLUDE globals.ink

-> LineOne

=== LineOne ===
Welcome to Purgatory, Nick Starlite.
+[Purgatory? Did I actually... Die?]
    -> LineTwo
=== LineTwo ===
Well, yes, you did.
But I bet you don't want to accept that, don't you?
You'll be glad to know that neither do I.
You have been fallen due to a mistake I've made.
The God of Light, Phanes, has agreed that a correction is to be made.
+[A correction? Of what kind?]
    -> LineThree
+[How would you correct... Death?]
    -> LineThree
== LineThree ===
We'll bring you back to life, obviously! How else do you solve death?
+[You can do that?]
    -> LineFour
=== LineFour ===
Yes! Though there will be one condition.
        +[There it is.]
            ~setTrigger("trigger2")
            Have a look.
        ->END