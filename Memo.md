# ÀÓ½Ã ¸Ş¸ğÀå

[µÚ·Î°¡±â](./README.md)

---
### 2026.07.07

- ToggleÀÇ SetIsOnWithoutNotify() ÇÔ¼ö - UnityEngine.UI <br>
°¢ º¯°æ¿¡ ´ëÇÑ ÀÌº¥Æ® ¹ßÇà ¾øÀÌ isOn °ª¸¸ º¯°æÇÏ´Â ÇÔ¼ö
```
_bgmToggle.SetIsOnWithoutNotify(!_soundManager.IsBgmMute);
```

-  ¾î¶² °´Ã¼ÀÇ Awake()°¡ ¸ÕÀú ½ÇÇàµÉÁö ºÒºĞ¸íÇÏ¹Ç·Î ¸ÕÀú ½ÇÇàµÇ±â ¿øÇÏ´Â Æ¯Á¤ °´Ã¼¿¡ ´ëÇØ <br>
Project Settings ¿¡¼­ ¿ì¼±¼øÀ§¸¦ ¼³Á¤ÇØÁÙ ¼ö ÀÖ´Ù.<br>
(´Ù¸¸, ³²¹ßÇÏÁö ¾Ê´Â °ÍÀÌ ÁÁ´Ù.)
	- ¢º Project Settings <br>¢º Script Execution Order <br>¢º ( + ) ¾ÆÀÌÄÜ Å¬¸¯ (Add script to custom order)<br>
    ¢º ¿øÇÏ´Â °´Ã¼¸í Å¬¸¯ÇÏ¿© Ãß°¡<br> ¢º ³·Àº ¼ıÀÚÀÏ ¼ö·Ï ´õ »¡¸® ½ÇÇàµÇ¹Ç·Î 0º¸´Ù ÀÛÀº ¼ıÀÚ·Î ¼³Á¤ (-1) <br>¢º
	 Apply<br> 
---
### 2026.07.14

[°³¼±Á¡] ÇÑ²¨¹ø¿¡ ¿©·¯ ÀûµéÀ» ¶§¸®´Â CotineousWeapon¿¡¼­ <br>
°¢ °³º° Enemy °´Ã¼¿¡ ´ëÇÑ ContinuousRoutine(°ø°İ ÄÚ·çÆ¾) ÇÔ¼ö¸¦ ÀÏÀÏÈ÷ ¼öÇàÇÏ¸é<br>
°úºÎÇÏ°¡ »ı±æ ¿ì·Á°¡ ÀÖ´Ù.

Ã¹¹øÂ° ¹æ¹ı - Event Action ±¸µ¶ / HashSet »ç¿ë

1) °ø°İ ÄÚ·çÆ¾Àº ½ÃÀÛ°ú µ¿½Ã¿¡ ½ÇÇà
2) OnTriggerEnter2D - ¹üÀ§ ³»¿¡ ´êÀº Àû HashSet¿¡ ´ã±â
3) OnTriggerExit2D - ¹üÀ§¸¦ ¹ş¾î³­ Àû HashSet¿¡¼­ »©±â
4) ÀûÀÌ ¹üÀ§ ³»¿¡¼­ Á×¾î nullÀÌ µÉ ½Ã HashSet¿¡¼­ ºüÁöµµ·Ï ¼³°è

µÎ¹øÂ° ¹æ¹ı - Physics.OverlapSphere »ç¿ë

1) ¹üÀ§ ³» ´êÀº ÀûÀÌ ¹è¿­·Î ¹İÈ¯µÇ°í ±× ¹è¿­ ¾È¿¡ Á¸ÀçÇÏ´Â Àûµé¸¸ ¶§¸®±â
2) ÀûÀÌ ¹üÀ§ ³»¿¡¼­ ¶§¸®±âµµ Àü¿¡ Á×¾î nullÀÌ µÉ ½Ã ¹«½ÃÇÏ°í ³Ñ¾î°¡µµ·Ï ¼³°è
= Ãæµ¹Ã¼ÀÇ »çÀÌÁî¸¦ º°µµ·Î °è»êÇÏ´Â ¹ø°Å·Î¿ò

-> Ã¹¹øÂ° ¹æ¹ı Ã¼ÅÃ


[¿¡·¯¹ß»ı]

InvalidOperationException: Collection was modified; enumeration operation may not execute. System.Collections.Generic.HashSet`1+Enumerator[T].MoveNext () (at <5dcb76a18315462e964f1bedee980471>:0) ContinuousWeapon+<ContinousRoutine>d__12.MoveNext () (at Assets/1.Scripts/1.Play/1.Upgrade/0.Weapon/2.ContinuousWeapon/ContinuousWeapon.cs:62) UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <254a04541d1c4a7b9e9c9e594cf72b4c>:0) 

¿øÀÎ: foreach¹®¿¡¼­ »õ·Î »ı¼ºÇÑ ¸®½ºÆ® 'enemies' ¸¦ ÂüÁ¶ÇØ¾ßÇÏ´Âµ¥ Çì½Ã¼Â '_enemies' À» ÂüÁ¶ÇÏ¿© ²¿¿©¹ö¸²


```
/// <summary>
    /// °ø°İ ÄÚ·çÆ¾
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinuousRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_atkSpan);

            List<Enemy> enemies = _enemies.ToList();

            foreach (var enemy in enemies)
                enemy.TakeHit(_dmg);
        }
    }
```

---
### 2026.07.16

Á÷·ÄÈ­(Serialization)

16 core 32Thread(4Ghz)

Á÷·Ä: Åë·Î(Stream)°¡ ÇÏ³ª - ¼±ÀÔ¼±Ãâ / ¼øÂ÷Àû
º´·Ä: Åë·Î°¡ ¿©·¯°³

<br><br>

CPU(Register)<br> 
¤Ó<br> 
Cache<br> 
¤Ó<br> 
RAM<br> 
¤Ó<br> 
I/O(ÀÔÃâ·ÂÀåÄ¡) : Keyboard / SSD / Network . . . .<br> 
¤¤ Inout¿ë Åë·Î<br> 
¤¤ Output¿ë Åë·Î
<br> 
ÃÖ¼Ò´ÜÀ§(Byte)·Î Åë·Î¸¦ Áö³ª°£´Ù - ByteStream

<br><br>

ÀÔ·Â¹æ½Ä)
text => Json : ÀĞ¾îµéÀÌ´Â ¼Óµµ´Â ´À¸®Áö¸¸ ÆíÁı ¿ëÀÌ
binary : ¹èÆ÷ ½Ã JsonÀ» binary·Î º¯È¯(¶â¾îº¼ ¼ö ¾ø´Ù)
<br> 
ByteStream¿¡ Åë°ú½ÃÅ°±â À§ÇØ Byte ´ÜÀ§·Î º¯È¯ÇÏ´Â ÀÛ¾÷ = Á÷·ÄÈ­

---
### 2026.07.21

[ÇÒ´ç ¿µ¿ª]<br> 
Automatic(ÀÚµ¿) - Stack<br> 
Static(Á¤Àû) - Data / ÃÊ±â°ªÀÌ ¾ø°Å³ª 0 È¤Àº nullÀÎ º¯¼ö - BSS (¿¹¾à°ø°£)<br> 
Dynamic(µ¿Àû) - Heap<br> 
<br> 
Á¤Àû »ı¼ºÀÚ º¸À¯ => static class<br> 
<br>
C -> ±â°è¾î : ÄÄÆÄÀÏ¸µ<br>
C# -> ByteCode(Áß°£¾ğ¾î) -> ±â°è¾î : ÀÎÅÍÇÁ¸®ÆÃ<br> 
<br> 
°ªÇü½ÄÀ» ÂüÁ¶Çü½ÄÀ¸·Î - Boxing<br> 
ÂüÁ¶Çü½ÄÀ» °ªÇü½ÄÀ¸·Î - Unboxing<br> 

<br> 

---
### 2026.07.22

```
// Sequential -> µ¥ÀÌÅÍ¸¦ ¼øÂ÷ÀûÀ¸·Î
// Pack = 1 -> 1 byte ¾¿ (¹ÙÀÌÆ®ÆĞ±ë)

[StructLayout(LayoutKind.Sequential, Pack = 1)] 
public struct WeaponData
{
    public WeaponType Type;
    public float Power;
    public float Speed;
    public float Durabillity;
    public float CriticalRatio;
}
```
<br><br>

- Dictionary

| key(string) - ±ÕÇü2ÁøÆ®¸® | Value(int) |- Pair => Array
|:--- |:--- |
| "Oak" | 100 | 
| "Elf" | 250 |
| "Dwarf" | 80 |
| "Human" | 200 |
| "Poring" | 150 |

<br>

2ÁøÆ®¸®<br>
  . Root<br>
 . . Branch<br>
.. .. Leaf<br>
¿ÏÀü2ÁøÆ®¸® , Æ÷È­2ÁøÆ®¸®
<br>

C# => AVRÆ®¸®

<br> 

---
### 2026.07.23
-> ÁÖ¸»¿¡ Ã£¾Æ¼­ °øºÎÇÒ °Í

#### ÀÌÁø Æ®¸®
- ¼øÈ¸ ¹æ½Ä(ÀüÀ§, ÁßÀ§, ÈÄÀ§)<br> 
<br> 
- ½Ã°£º¹Àâµµ (ÃÖ¾Ç O)
```
O(1) > O(logn) > O(n) > O(n*logn) > O(n^2) > O(n^3)
```
<br> 

- O(n)
```
for (int i = 0; i < 101; i++)
{
    if (i == 100) 
        break;
}
```
<br> 

- O(n^2^)
```
for (int y = 0; y < n; y++)
{
    for (int x = 0; x < n; x++)
    {
            
    }
}
```
- ÀÌÁøÅ½»ö - O(logn)
    - ÀÌÁøÅ½»ö Æ®¸® - O(logn)
- ÀÌÁøÆ®¸® Á¾·ù
    - ¿ÏÀüÀÌÁøÆ®¸®:  ¸¶Áö¸· ·¹º§À» Á¦¿ÜÇÏ°í ¸ğµç ·¹º§ÀÌ ¿ÏÀüÈ÷ Ã¤¿öÁ® ÀÖÀ¸¸ç,<br>
                    ¸¶Áö¸· ·¹º§ÀÇ ³ëµåµéÀº ¿ŞÂÊºÎÅÍ Â÷·Ê´ë·Î Ã¤¿öÁø ÇüÅÂÀÔ´Ï´Ù.
    - Æ÷È­ÀÌÁøÆ®¸®: ¸ğµç ·¹º§ÀÌ ºóÆ´¾øÀÌ ²Ë Âù ÇüÅÂ. ¸¶Áö¸· ·¹º§À» Á¦¿ÜÇÑ ¸ğµç ³ëµå°¡ 2°³ÀÇ ÀÚ½ÄÀ» °¡Áö¸ç,<br> 
                   ÀÙ(Leaf) ³ëµå´Â ¸ğµÎ µ¿ÀÏÇÑ ±íÀÌ¿¡ À§Ä¡.
- Èü(Heap)
- ±ÕÇüÀÌÁøÆ®¸®: ¸ğµç ³ëµåÀÇ ÁÂ¿ì ¼­ºêÆ®¸® ³ôÀÌ Â÷ÀÌ°¡ ÃÖ´ë 1 ÀÌÇÏ
    - AVL Tree: ³ôÀÌ Â÷ÀÌ¸¦ ¾ö°İÇÏ°Ô 1 ÀÌÇÏ·Î ÁöÅ°¸ç °ü¸®.
    - Red Black Tree: ³ëµå¿¡ »ö»ó Á¤º¸¸¦ µÎ¾î Á¶±İ ´õ À¯¿¬ÇÏ°Ô ±ÕÇüÀ» ¸ÂÃß¸ç ³Î¸® ¾²ÀÔ´Ï´Ù.
    - B Tree: ÀÌÁø Æ®¸®¸¦ È®ÀåÇØ ÇÏ³ªÀÇ ³ëµå°¡ 2°³ ÀÌ»óÀÇ ÀÚ½ÄÀ» °¡Áú ¼ö ÀÖµµ·Ï ¸¸µç ÀÚ°¡ ±ÕÇü Å½»ö Æ®¸®.<br>
              ÁÖ·Î µ¥ÀÌÅÍº£ÀÌ½º¿Í ÆÄÀÏ ½Ã½ºÅÛ¿¡¼­ ´ë¿ë·® µ¥ÀÌÅÍ¸¦ È¿À²ÀûÀ¸·Î ´Ù·ç±â À§ÇØ »ç¿ë.<br>
              ¾ö¹ĞÈ÷ ¸»ÇÏ¸é B-Æ®¸®´Â ÀÚ½Ä ¼ö°¡ 2°³·Î Á¦ÇÑµÈ ÀÌÁø Æ®¸®°¡ ¾Æ´Ï¶ó,<br>
              ¿©·¯ °³ÀÇ ÀÚ½Ä°ú Å°¸¦ °¡Áú ¼ö ÀÖ´Â ´ÙÂ÷(Multi-way) ±ÕÇü Æ®¸®.
<br><br>
#### ÀÚ·á±¸Á¶ (ÇĞ¹®Àû)
- ¼±Çü
    - ¹è¿­(Array)
        - °íÁ¤Çü
        - °¡º¯Çü(List)
            - ¿¬°á ¸®½ºÆ®(Linked List)
                - ÀÌÁß ¿¬°á ¸®½ºÆ®(Duplex)
                - ¿øÇü ¿¬°á ¸®½ºÆ®(Circular)
- ½ºÅÃ(Stack)
- Å¥(Queue)
    - ½ºÅ©·Ñ(Scroll)
    - ½©ÇÁ(Shelf)
    - µ¥Å¥(Dequeue)
    - Èü(Heap)
<br><br>
- ºñ¼±Çü
    - Æ®¸®
        - ÀÏ¹İÇü Æ®¸®
        - nÁøÇü Æ®¸®
            - 2Áø Æ®¸®(Binary)
                - ¼øÈ¸(Àü, Áß, ÈÄ)
                - 2Áø Å½»ö(Binary Search)
                - 2ÀÎ Å½»öÆ®¸®(BST)
                - Èü(Heap)
                - ±ÕÇü 2ÁøÆ®¸®
                    - AVL, RB, B
            - 4Áø Æ®¸®(Quad)
            - 8Áø Æ®¸®(Oct)
    - ±×·¡ÇÁ
        - Å½»ö
            - ±íÀÌ ¿ì¼± Å½»ö(DFS)
            - ³Êºñ ¿ì¼± Å½»ö(BFS)
        - À§»ó Á¤·Ä(Topology Sort)
        - ÃÖ¼Ò ºñ¿ë Æ®¸®(MST)
            - ÇÁ¸²(Prim)
            - Å©·ç½ºÅ»(Kruskal)
            - ¼Ö¸°(Sollin) - º¸·çºÎÄ«(Boruvka)
- ÆÄÀÏ
    - ÅØ½ºÆ® ÆÄÀÏ(json, XML)
    - ¹ÙÀÌ³Ê¸® ÆÄÀÏ
    - Á÷·ÄÈ­(Serialization)
<br><br>
#### ¾Ë°í¸®Áò
- º¹Àâµµ
    - ½Ã°£ º¹Àâµµ
    - °ø°£ º¹Àâµµ
- Å½»ö
    - ¼øÂ÷ Å½»ö
    - 2Áø Å½»ö
    - ¹®ÀÚ¿­ Å½»ö
- Á¤·Ä
    - O(n^2)
        - ¹öºí Á¤·Ä
        - ¼±ÅÃ Á¤·Ä
        - »ğÀÔ Á¤·Ä
    - O(m*logn)
        - º´ÇÕ Á¤·Ä
        - Äü Á¤·Ä
- ±æÃ£±â(ÃÖ´Ü°Å¸®)
    - ´ÙÀÍ½ºÆ®¶ó(µ¥ÀÌÅ©½ºÆ®¶ó)
    - A-Star(A*)
        - Nevigation Mesh
    - º§¸¸-Æ÷µå - O(n^2)
    - ÇÃ·Î¿öµå-¿ö¼È - O(n^3)

- ¼³°è¹æ½Ä(±¸»ó¹æ½Ä)
    - ºĞÇÒ / Á¤º¹
        - ºĞÇÒµÈ ¹®Á¦³¢¸® ¿µÇâÀ» ÁÖÁö ¾ÊÀ½ -> º´·Ä ÇÁ·Î±×·¡¹Ö °¡´É
- µ¿Àû ÇÁ·Î±×·¡¹Ö
    - ¸Ş¸ğÀÌÁ¦ÀÌ¼Ç
    - º´·Ä ÇÁ·Î±×·¡¹Ö X
- Å½¿å(Greedy)
- ¹éÆ®·¡Å·(Back Tracking)
<br> 

Âü°íÀÚ·á: https://rosweet-ai.tistory.com/55

---
### 2026.07.27
[¿ë¾î]<br> 
LOD - Level Of Detail<br> 

[´Ü­sÅ°]<br> 
ctrl + shift = ¹Ù´Ú¿¡ µü ºÙ¾î¼­<br> 
ctrl = ¼³Á¤ÇÑ ´ÜÀ§ ¸¸Å­¸¸ ¿òÁ÷ÀÓ<br> 
v = Á¤Á¡¿¡ µü ¸ÂÃç¼­<br> 

ProjectSettings - Quality - Level of Details(ÃÖÇÏ´Ü) <= LOD ÀÏ°ı ¼³Á¤<br> 

//////////////////////////<br> 

[°³¼±Á¡] ·¹º§ ¾÷ ½Ã ¿¬¼ÓµÈ °ø°İ Å° ÀÔ·Â ½Ã ¾÷±×·¹ÀÌµå È­¸éÀÌ ¹Ù·Î ³Ñ¾î°¡¹ö¸®´Â Çö»ó °³¼±



[¿¡·¯¹ß»ı]

InvalidOperationException: Collection was modified; enumeration operation may not execute. System.Collections.Generic.HashSet`1+Enumerator[T].MoveNext () (at <5dcb76a18315462e964f1bedee980471>:0) ContinuousWeapon+<ContinousRoutine>d__12.MoveNext () (at Assets/1.Scripts/1.Play/1.Upgrade/0.Weapon/2.ContinuousWeapon/ContinuousWeapon.cs:62) UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <254a04541d1c4a7b9e9c9e594cf72b4c>:0) 

¿øÀÎ: foreach¹®¿¡¼­ »õ·Î »ı¼ºÇÑ ¸®½ºÆ®¸¦ ÂüÁ¶ÇØ¾ßÇÏ´Âµ¥ Çì½Ã¼ÂÀ» Âü°íÇÏ¿© ²¿¿©¹ö¸²

//////////////////////////<br> 

1. À¯´ÏÆ¼ ¿¡µğÅÍ¿¡¼­ ¿¡¼ÂÀº ÆÄÀÏ·Î °£ÁÖ<br>
-> ÀÔÃâ·Â ÀåÄ¡¸¦ °ÅÄ£´Ù°í °£ÁÖ<br>
-> ÀÔÃâ·Â ÀåÄ¡´Â Á÷·ÄÈ­¸¦ ÅëÇØ¾ß µÇ±â ¶§¹®¿¡ Á÷·ÄÈ­µÈ Å¬·¡½º ¹× º¯¼ö¸¸ ¿¡µğÅÍ¿¡ Ãâ·Â<br>
-> HideInspcetor: Á÷·ÄÈ­¿Í »ó°ü ¾øÀÌ ¿¡µğÅÍ »ó¿¡¼­ ÆíÁıÇÒ ¼ö ¾øµµ·Ï ¼û±â´Â ±â´É<br>

2. ¸â¹ö º¯¼ö<br>
 -> publicÀº °ø°³µÇ¹Ç·Î ÀÚµ¿ Á÷·ÄÈ­µÇ´Â º¯¼ö<br>
 -> public º¯¼ö´Â NonSerialized·Î Á÷·ÄÈ­ ÇÏÁö ¾Ê´Â °ÍÀÌ °¡´É<br>
 -> privateÀº °ø°³µÇÁö ¾ÊÀ¸¹Ç·Î Á÷·ÄÈ­ ¾ÊµÊ<br>
 -> privateÀÇ °æ¿ì Á÷·ÄÈ­¸¦ ÇÏ°í ½ÍÀ»¶§ SerializeField ¾ÖÆ®¸®ºäÆ® »ç¿ë<br>

 - µ¨¸®°ÔÀÌÆ®(delegate)
 - 
<br> 

---

### 2026.07.28

<br> 

---

### 2026.07.29

<br> 

ºôº¸µåÃ³¸®

---

### 2026.07.30

<br> 

#### ¶÷´Ù Æ¯¼º Á¤¸®
1. µ¨¸®°ÔÀÌÆ®(delegate)¸¦ ÅëÇØ¼­¸¸ Á¤ÀÇ °¡´É
    - µ¨¸®°ÔÀÌÆ®ÀÇ Å¸ÀÔÀ» ÅëÇØ ÆÄ¶ó¹ÌÅÍ Å¸ÀÔ°ú ¸®ÅÏ Å¸ÀÔÀ» »ı¼º

---

### 2026.07.31
---
[µÚ·Î°¡±â](./README.md)