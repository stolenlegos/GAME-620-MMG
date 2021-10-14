using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager {
  public static string[] KillProposalDialogue = new string[] {
    //0
    "You there! Adventurer! I need your help with something. The city has been dealing with a lot of monster issues lately and the guardsmen are stretched thin.",
    //1
    "I have gotten word there are some dragons that need to be dealt with. Could you help us out?",
    //2
    "I have gotten word there are some bears that need to be dealt with. Could you help us out?",
    //3
    "I have gotten word there are some ghosts that need to be dealt with. Could you help us out?",
    //4
    "I have gotten word there are some rats that need to be dealt with. Could you help us out?",
    //5
    "I have gotten word there are some slimes that need to be dealth with. Could you help us out?",
    //6
    "You've handled yourself very well before.",
    //7
    "You were a huge help the last time I needed you.",
    //8
    "Maybe you can make up for not following through with last time.",
    //9
    "Perhaps this time you will keep your word"
  };

  public static string KillAcceptDialogue = "Great! When you have dealt with the problem, report back to me.";

  public static string[] KillDenyDialogue = new string[] {
    //0
    "Could you please help?",
    //1
    "It would mean a lot to us if you could do it.",
    //2
    "We will make it up to you at a future time!",
    //3
    "Fine. I will find another adventurer"
  };

  public static string KillReturnDialogue = "Well done! You have been a great help to the city!";

  public static string[] KillBeforeCompleteDialogue = new string[] {
    //0
    "Something like this should be easy for you.",
    //1
    "Same as the last time. Make sure they are all dead.",
    //2
    "Monsters should run scared as soon as they hear it's you coming.",
    //3
    "You could probably do it in your sleep."
  };




  public static string[] DeliveryProposalDialogue = new string[] {
    //0
    "Hello Adventurer! Fine day we are having, isn't it? I was wondering, would you be able to help me with something?",
    //1
    "I need help with a delivery. Could you take this sword over to the mail post? Hope that isn't too much trouble.",
    //2
    "I need help with a delivery. Could you take these jewels over to the mail post? Hope that isn't too much trouble.",
    //3
    "I need help with a delivery. Could you take this shield over to the mail post? Hope that isn't too much trouble.",
    //4
    "I need help with a delivery. Could you take this bread over to the mail post? Hope that isn't too much trouble.",
    //5
    "I need help with a delivery. Could you take this corn over to the mail post? Hope that isn't too much trouble.",
    //6
    "I need help with a delivery. Could you take this book over to the mail post? Hope that isn't too much trouble.",
    //7
    "You were a great help last time!",
    //8
    "You made my day so much easier before!",
    //9
    "I understand if it is too much for you.",
    //10
    "Sorry if I am being a bother. I know you do a lot."
  };

  public static string DeliveryAcceptDialogue = "Wonderful! Let me know when you have dropped it off.";

  public static string[] DeliveryDenyDialogue = new string[] {
    //0
    "It won't take you much time.",
    //1
    "I'm very swamped today, it would be kind of you to do it.",
    //2
    "This birthday gift will be late otherwise.",
    //3
    "Oh, okay. That's fine. I should be able to find someone else."
  };

  public static string DeliveryReturnDialogue = "Thank you very much for doing that!";

  public static string[] DeliveryBeforeCompleteDialogue = new string[] {
    //0
    "I think you know the way.",
    //1
    "I trust you won't get lost.",
    //2
    "I'd give you directions, but I think you know where it is.",
    //3
    "You'll be taking it to the mail post."
  };




  public static string[] FetchProposeDialogue = new string[] {
    //0
    "Adventurer! Adventurer! You are an adventurer, yes? I could really use your help!",
    //1
    "I need help finding my sword. I was in a rush earlier and dropped it. Could you help me find it?",
    //2
    "I need help finding my jewels. I was in a rush earlier and dropped it. Could you help me find it?",
    //3
    "I need help finding my shield. I was in a rush earlier and dropped it. Could you help me find it?",
    //4
    "I need help finding my bread. I was in a rush earlier and dropped it. Could you help me find it?",
    //5
    "I need help finding my corn. I was in a rush earlier and dropped it. Could you help me find it?",
    //6
    "I need help finding my book. I was in a rush earlier and dropped it. Could you help me find it?",
    //7
    "You saved me so much stress last time!",
    //8
    "You're always so good about finding my things!",
    //9
    "Maybe this time you'll actually help me",
    //10
    "I probably can't rely on you though..."
  };

  public static string FetchAcceptDialogue = "Really?! Thank you so much! I will wait here for you to return it.";

  public static string[] FetchDenyDialogue = new string[] {
    //0
    "If I don't find it now, it will be lost forever!",
    //1
    "Please, friend, it was a family heirloom.",
    //2
    "If I can't get it back my kid will have a disappointing birthday.",
    //3
    "And you call yourself an Adventurer?! I will find someone else!"
  };

  public static string FetchReturnDialogue = "Thank you so much for doing this! I'll try not to lose it next time.";

  public static string[] FetchBeforeCompleteDialogue = new string[] {
    //0
    "I'm sure you'll find it like before.",
    //1
    "You've found my things before, right?",
    //2
    "I think you've helped me before, haven't you?",
    //3
    "I know you know how to find things."
  };
}
