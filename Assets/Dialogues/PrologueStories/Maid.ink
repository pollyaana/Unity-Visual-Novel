VAR storyId = 2

VAR yourName = ""
VAR rightCharacter = ""
VAR time = ""
VAR showTime = ""
VAR isLetter = false
VAR isBook = false
VAR background = "покои"
VAR leftCharacter = "Элен"
VAR characterName = "Элен"
VAR cloudStatus = 1
VAR panel = ""
VAR wordLink = ""
VAR music = ""
{yourName}, где тебя носит? Скорее иди ко мне, уже пора собираться.

~background="коридор"
~leftCharacter = "Служанка"
~characterName = yourName
~cloudStatus = 1
Иду, княжна!

~background="покои"
~leftCharacter = "Элен"
~characterName = "Элен"
~cloudStatus = 1
Быстрее!

~leftCharacter = "Служанка"
~rightCharacter = "ЭленЗеркало"
~characterName = ""
~cloudStatus = 0
Через минуту Вы уже стояли в покоях молодой и прекрасной княжны. Она стояла у зеркала пыталась что-то сделать со своими волосами. В отражении Элен увидела Вас и с неким   раздражением сказала:
~rightCharacter = "Элен"
~characterName = "Элен"
~cloudStatus = 2
Наконец таки! Явилась! Сколько тебя можно ждать?! Выходить уже через час, а я совсем не готова. Итак, где моё платье?

~characterName = yourName
~cloudStatus = 1
Какое платье, Елена Васильевна?

~cloudStatus = 2
~characterName = "Элен"
Бестолочь! Я же просила тебя забрать моё платье из ателье!
    +[Я совсем забыла, княжна...]
                        -> Angry_Elen
     +[Вы мне ничего не говорили...]
                        -> B
    +[А Вы про это платье?]
                        -> C

=== Angry_Elen ===
~cloudStatus = 2
~characterName = "Элен"
Я же говорю, бестолочь! Провалить такое простое задание? И в чём я пойду к тётушке Шерер?
~characterName = yourName
~cloudStatus = 1
Может Вы выберите платье из Вашего шкапа? У вас много прекрасных нарядов. 
~characterName = "Элен"
~cloudStatus = 2
Нарядов то может быть и много, но во всех я уже появлялась в свете. Это будет крайне неприлично явиться на вечер в том платье, в котором я, например, была давеча у князя Н. Я опозорю себя, своего papa...

~characterName = "Элен"
~cloudStatus = 2
Который час, {yourName}?

~characterName = yourName
~cloudStatus = 1
Почти полдень, барышня.
~characterName = "Элен"
~cloudStatus = 2
Ещё есть надежда! Пошли быстрее Тихона в ателье. Может ещё успеет. Пусть владельцу даст 3 рубля, если опоздает. Эти деньги вычтем из твоего жалования.
    +[Тихон не сможет, он же вчера упал, повредил себе ногу.]
                        -> A2_1
     +[Уже бегу!]
                        -> A2
=== A2 ===
~characterName = ""
~cloudStatus= 0
Через некоторое время Вы вернулись к княжне и сказали:
    +[Тихон болен.]
                        -> A2_1
     +[Тихон уже в пути!]
                        -> A2_2
-->END
-> Звали

=== A2_1 ===
~characterName = "Элен"
~cloudStatus = 2
Я пропала! Надо скорее сказать папеньке, что я больна и не смогу навестить любимую Анну Павловну! Нет, я не могу пропустить этот вечер, мне обязательно нужно там быть.
Итак, либо ты сейчас сама бежишь в ателье за моим платьем, либо я устраиваю скандал и тебя тут же выгонят из этого дома.
~characterName = yourName
~cloudStatus = 1
Нет, прошу не выгоняйте меня, я сделаю, что угодно!
~characterName = "Элен"
~cloudStatus = 2
То то же. Зови Фёклу, пусть она поможет мне сделать причёску,  а ты беги за платьем. Если через час я не буду в нём, то можешь искать себе место на улице.
-> Sad

=== Sad ==
~characterName = ""
~cloudStatus = 0
~leftCharacter="Служанка"
~rightCharacter = "quick"
~background="лестница"
В расстройстве Вы спустились вниз по лестнице, на глазах наворачивались слёзы от обиды и несправедливости. На лестнице Вы услышали голос князя Василия. Он звал Вас к себе. Вы быстро вытерли глаза рукавом, и отправились к князю.
-> Звали

=== Звали ===
~background="Кабинет"
~characterName = yourName
~rightCharacter="Князь"
~cloudStatus = 1
Звали, Василий Сергеевич? 
~characterName = "Кн. Василий"
~cloudStatus = 2
Да, {yourName}. Что там долго ещё собираться моей дочурке? Если не выйдем через час, то мы неприлично опоздаем.
~characterName = yourName
~cloudStatus = 1
Да, к этому времени она будет готова.
~characterName = "Кн. Василий"
~cloudStatus = 2
Это хорошо.
~characterName = yourName
~cloudStatus = 1
Это всё, князь? Я могу идти?
~characterName = "Кн. Василий"
~cloudStatus = 2
Нет, {yourName}, подожди немного. Звал я тебя вот по какому поводу... Видишь ли, всякие слухи ходят о моём сыне Анатоле, что человек он плохой, многим женщинам навредил, слышала ли ты об этом?
    +[Да,князь.]
                        -> answ1
     +[Нет, князь]
                        -> answ2

=== answ1 ===
~characterName = "Кн. Василий"
~cloudStatus = 2
И что ты думаешь? Врут это люди или нет?
~characterName = yourName
~cloudStatus = 1
Не могу знать, Василий Сергеевич.
~characterName = "Кн. Василий"
~cloudStatus = 2
Так и знал, проку от этого мало. Но вот что, {yourName}, если слова эти подтвердятся, немедленно сообщи мне. Будем воспитывать этого дурака беспокойного.
~characterName = yourName
~cloudStatus = 1
Хорошо, князь. Могу ли я идти?
~characterName = "Кн. Василий"
~cloudStatus = 2
~music = "карета"
Да, теперь ты свободна.
-> common
=== answ2 ===
~characterName = "Кн. Василий"
~cloudStatus = 2
Правильно, что не слышала. Многое люди говорят, да не всё правда...
~characterName = "Кн. Василий"
~cloudStatus = 2
{yourName}, если что-то услышишь, увидишь, то немедленно сообщи мне. 
~characterName = yourName
~cloudStatus = 1
Как скажете, князь. Обязана ли я ещё чем то?
~characterName = "Кн. Василий"
~cloudStatus = 2
~music = "карета"
Нет, иди.
-> common
--> END


=== common ===
~characterName = ""
~cloudStatus = 0
~background="ателье"
~rightCharacter = "quick"
~music = "стук"
Вы вышли из кабинета князя, быстро спустились вниз. Путь в ателье был долгим, поэтому Вы решили не идти, а бежали, так как знали, что если Вас вышвырнут из этого дома, другого места Вам не найти в Петербурге. Через полчаса Вы уже стояли у ателье.
Но дверь была закрыта, Вы начали стучать. Из ателье к Вам на встречу вышел мужчина.
~rightCharacter="Мужчина"
~characterName = "Мужчина"

~cloudStatus = 2
Девушка, зачем Вы мне ломаете дверь!
~characterName = yourName
~cloudStatus = 1
Как ломаю, она ведь была закрыта?
~characterName = "Мужчина"
~cloudStatus = 2
Если не в ту сторону дверь открывать, то, конечно, она будет закрыта. Что Вы хотели?
~characterName = yourName
~cloudStatus = 1
Я хочу забрать платье княжны Курагиной.
~characterName = "Мужчина"
~cloudStatus = 2
А, то, нежно-голубое?
~characterName = yourName
~cloudStatus = 1
Да, его, наверное.
~characterName = "Мужчина"
~cloudStatus = 2
Наверное-наверное. Столько платье пошьют, а потом и не помнят какого они цвета. Сейчас принесу.
~characterName = "Мужчина"
~cloudStatus = 2
Вот Ваше платье. С Вас 5 рублей за работу.
~characterName = ""
~cloudStatus = 0
Вы забыли, что работа швеи не оплачена, а деньги Вы, конечно, оставили в доме.
~characterName = yourName
~cloudStatus = 1
У меня нет с собой денег... Может Вы отдадите мне это платье просто так? Завтра я занесу деньги!
~characterName = "Мужчина"
~cloudStatus = 2
Ой, эту песню я знаю. Нет, милочка, нет денег — нет платья. Как появятся, приходи.
~characterName = yourName
~cloudStatus = 1
Но меня выгонят из дома, если я его не принесу.
~characterName = "Мужчина"
~cloudStatus = 2
Хватит давить на жалость, я уже всё сказал. Уходите.
~characterName = ""
~cloudStatus = 0
~background = "ателье снаружи"
~rightCharacter = "quick"
Вы поняли, что Вам уже можно не возвращаться в дом Курагиных. Вы сели на обочину и стали горько плакать от безысходности
~characterName = "Незнакомец"
~cloudStatus = 2
~rightCharacter="Незнакомец"
Mademoiselle, что Вас так опечалило?
~characterName = yourName
~cloudStatus = 1
Проходите мимо. Вам всё равно нет никакого дела до меня.
~characterName = "Незнакомец"
~cloudStatus = 2
Прошу, расскажите мне. Может я смогу Вам чем-нибудь помочь.
~characterName = yourName
~cloudStatus = 1
Если у Вас есть 5 рублей, то они спасут мою жизнь.
~characterName = yourName
~cloudStatus = 1
Незнакомец немного порылся в карманах и достал деньги.
~characterName = "Незнакомец"
~cloudStatus = 2
Вот,держите. Зачем они Вам нужны я не знаю и спрашивать не буду. Держите.
~characterName = ""
~cloudStatus = 0
~background = "ателье внутри"
~rightCharacter = "quick"
Вы не верили своим глазам. Этот человек дал Вам деньги, он спас Вас. Теперь Вы сможете принести платье своей молодой княжне. Вы взяли бумажки, поблагодарили незнакомца и забежали в ателье.
~characterName = "Мужчина"
~cloudStatus = 2
~rightCharacter = "Мужчина"
Девушка, я же Вам сказал. Нет денег — нет платья, уходите немедленно отсюда.
~characterName = yourName
~cloudStatus = 1
Подождите, вот, вот  5 рублей, держите.
~characterName = "Мужчина"
~cloudStatus = 2
Ну вот и зачем нужно было спектакль разыгрывать? Возьмите Ваше платье. До свидания!
~characterName = yourName
~cloudStatus = 1
Спасибо!
~characterName = ""
~cloudStatus = 0
~background = "ателье снаружи"
~rightCharacter = "quick"
Обратный путь Вы преодолели очень быстро. Видимо, случайная встреча Вашего спасителя благотворно повлияла на Вас. Вы вновь поверили в то, что Бог Вас любит и не оставит в беде.
~characterName = yourName
~cloudStatus = 1
~rightCharacter=""
Какая же я неблагодарная! Даже не узнала имени того доброго человека! Как же мне за него молится?
~characterName = ""
~cloudStatus = 0
~background="покои"
Вы вошли в дом Курагиных и сразу побежали к княжне. Войдя в комнату, Вы увидели, что она уже почти готова.
~characterName = "Элен"
~rightCharacter="Элен"
~cloudStatus= 2
Ну что, ты принесла моё платье?
~characterName = yourName
~cloudStatus= 1
Да, Елена Васильевна!
~characterName = "Элен"
~cloudStatus= 2
Сколько раз я тебя просила меня не называть так. Я Элен!
~characterName = yourName
~cloudStatus= 1
Точно, Элен Васильевна!
~characterName = "Элен"
~cloudStatus= 2
Ох, ладно. Давай платье.
~characterName = ""
~cloudStatus= 0
~rightCharacter="Элен2"
Вы отдали платье, через несколько мгновений оно уже струилось по фигуре Элен Курагиной. Какой же красивой она была. Лицо, белые плечи, грудь, спина — вся она была похожа на древнегреческую богиню. Именно ради такой красоты мужчины развязывают войны.
~characterName = "Элен"
~cloudStatus= 2
Прекрасно! Magnifique!
~leftCharacter="Князь"
~characterName = ""
~cloudStatus= 0
В комнате появился князь Василий.
~cloudStatus= 1
~characterName = "Кн. Василий"

И в кого же ты такая красивая, моя Елена Прекрасная?!
~characterName = "Элен"
~cloudStatus= 2
Papa! Мы уже можем ехать?
~characterName = "Кн. Василий"
~cloudStatus= 1
Да, карета уже готова. Пойдём дорогая.
~characterName = "Элен"
~cloudStatus= 2
Хорошо. Papa, могу я взять с собой нашу служанку? Мне нужна будет её помощь, когда мы приедем к Анне Павловне?
~characterName = "Кн. Василий"
~cloudStatus= 1
Если ты этого хочешь, то, конечно.
~characterName = "Элен"
~cloudStatus= 2
{yourName}, поедешь со мной.
~characterName = yourName
~cloudStatus= 1
~leftCharacter = "Служанка"
Я?
~characterName = "Элен"
~cloudStatus= 2
~music = "карета"
Да, ты. Быстрее пойдём, или я возьму кого-нибудь другого.
~rightCharacter="quick"
~cloudStatus= 0
~characterName = ""
~background="каретаВнутри"
Вы сели в карету и отправились в дом Анны Шерер. Вы не могли поверить, что Вы увидите её, светскую львицу, фрейлину императрицы Марии Федоровны, а может сегодня Вам улыбнётся удача и Вы сможете увидеть и саму царицу. Мысли о том, как худший день превратился в самый лучший не покидали Вас
~characterName = "Кн. Василий"
~cloudStatus= 2
~leftCharacter = "quick"
~rightCharacter="Князь"
~music = "микс"
Вот и приехали. Пойдёмте.
~rightCharacter="quick"
~cloudStatus= 0
~characterName = ""
~background="домВнутри"
Дом Анны Шерер Вам показался огромным. Всё в нём Вам представлялось необычайным, и  даже самые простые вещи для Вас были наполнены невероятной красотой.
~leftCharacter = "Служанка"
~rightCharacter = "Элен2"
~characterName = "Элен"
~cloudStatus= 2
Ну, чего ты рот разинула? Помоги мне привести себя в порядок после дороги.
~characterName = "Элен"
~cloudStatus = 2
Всё, теперь ты свободна. Пусть Кузьмич отвезёт тебя обратно.
~characterName = yourName
~cloudStatus = 1
А нельзя ли мне остаться с Вами?
~characterName = "Элен"
~cloudStatus = 2
И что ты тут будешь делать? Нет. Нельзя. Иди.
~characterName = yourName
~cloudStatus = 1
Может я ещё нужна Вашему папеньке?
~characterName = "Элен"
~cloudStatus = 2
Нет, он уже в зале, разговаривает с Анной Павловной. Иди скорее. Дай мне настроиться.
~characterName = yourName
~cloudStatus = 1
Хорошо, княжна.
~characterName = "Элен"
~cloudStatus = 2
Ах, да. Скажи Кузьмичу, чтобы дома он не задерживался. Ждём его к 10 часам.
~characterName = yourName
~cloudStatus = 1
Будет сделано.
~characterName = yourName + " (про себя)"
~cloudStatus = 1
~rightCharacter="quick"
~music = "карета"
Вот бы я была княжной. Ходила бы на такие балы, жила бы в таких домах, каждый час шила бы себе новое платье, пока не закончилось бы место в шкапу...
~characterName = ""
~cloudStatus = 0
~background="карета"
~rightCharacter="Пьер"
Вы вышли на улицу и уже почти сели в карету, и тут Вы увидели своего спасителя: массивный молодой человек со стриженной головой. Вы хотели подбежать к нему, чтобы вновь поблагодарить его и узнать его имя. Но передумали. У Кузьмича Вы спросили:
~characterName = yourName
~cloudStatus = 1
~rightCharacter="quick"
А кто это молодой человек, Кузьмич? Ты всех знаешь, скажи, пожалуйста.
~characterName = "Кузьмич"
~cloudStatus = 2
~rightCharacter="Кузьмич"
Ну ты мать, даёшь. Это же Пьер Безухов. У наших господ уже целую неделю живёт. Родственник им там какой-то.
~characterName = yourName
~cloudStatus = 1
Я его не видела раньше.
~characterName = "Кузьмич"
~cloudStatus = 2
Конечно, не видела. Ты ведь только с барышней общаешься и усё. Да и сам он странный какой-то. Не похож на других господ.
~characterName = "Кузьмич"
~cloudStatus = 2
А что понравился тебе что ли? 
    +[Он мне помог.]
                        -> Help
     +[Промолчать]
                        -> END
     +[Поехали домой, Кузьмич.]
                        -> END
--> END
=== Help ===
~characterName = "Кузьмич"
~cloudStatus = 2
И чем же?
~characterName = yourName
~cloudStatus = 1
Не твоё дело. Поехали домой.
->END
=== A2_2 ===
~characterName = "Элен"
~cloudStatus = 2
Отлично! Пока он в поисках платья, начисти мои туфли и найди те украшения с бабочками. Ах, да и Фёклу позови. Пусть она мне сделает причёску.
~characterName = yourName
~cloudStatus = 1
Как скажете, княжна.
->Poklon
-->END

=== Poklon === 
~characterName=""
~rightCharacter="quick"
~cloudStatus= 0
~background="коридор"
Вы поклонились и пошли за Фёклой, но вдруг Вы услышали громкий разговор в кабинете князя Василия. Вы бессознательно подошли ближе к двери.
~characterName= "Кн. Василий (за дверьми)"
~cloudStatus= 2
И вот как мне с вами разговаривать?! Один дурак покойный, второй беспокойный! Сил моих нет!
~characterName=""
~cloudStatus= 0
Шаги князя Василия приближались к двери. Она открылась. Вы чуть не упали в дверной проём.
~characterName= "Кн. Василий"
~cloudStatus= 2
~rightCharacter="Князь"
Кн. Василий: Вот ещё одно недоразумение! Если подслушиваешь, так улавливай суть и скрывайся с глаз, когда услышала шаги, а не стой пока тебя разоблачат. А ну, пошла вон отсюда!
    +[Молча уйти.]
                        -> Fekla
     +[Уже ухожу]
                        -> Fekla
=== Fekla ===
~characterName=""
~rightCharacter="quick"
~cloudStatus= 0
От нелепой ситуации, в которую Вы попали, Вы забыли, что Элен просила Вас сделать. Только когда Вы увидели Фёклу, Вы вспомнили о своих обязанностях.
~characterName = yourName
~cloudStatus= 1
~rightCharacter="Фёкла"
Вот память девичья! Фёкла, тебя барышня зовёт, чтобы ты сделала ей причёску.
~characterName = "Фёкла"
~cloudStatus= 2
Хорошо, {yourName}. Сейчас поднимусь к ней.
~characterName = yourName
~cloudStatus= 1
Если она будет не в духе, не сердись на неё. Она тебя попросила позвать 15 минут назад, а я совсем забыла.
~characterName = "Фёкла"
~cloudStatus= 2
Ну ничего страшного. Она у нас и когда в хорошем настроение, то тоже может быть не в духе.
~characterName = ""
~cloudStatus= 0
~leftCharacter = ""
~rightCharacter =""
~background="карета"
Через час карета была уже подана. Все были готовы к поездке, кроме Элен. Её платья так и не было.
~background="покои"
~rightCharacter ="Элен"
~leftCharacter = "Служанка"
~characterName = "Элен"
~cloudStatus= 2
{yourName}, где моё платье?!
~characterName = yourName
~cloudStatus= 1
Я отправила Тихона, но он ещё не вернулся...
~characterName = "Элен"
~cloudStatus= 2
И что мне делать? Уже пора выезжать.
~characterName = yourName
~cloudStatus= 1
Едьте так.
~characterName = "Элен"
~cloudStatus= 2
Как так? В одном нижнем белье? Ты издеваешься что ли?
~characterName = "Элен"
~cloudStatus= 2
Papa! Papa!
~characterName = yourName
~cloudStatus= 1
Нет, пожалуйста, не зовите князя. Я уже слышу стук копыт, это Тихон, точно Тихон. Сейчас я принесу Ваше платье.
~characterName = "Элен"
~cloudStatus= 2
Давай, пошевеливайся!!
~background="каморка"
~characterName=""
~cloudStatus= 0
~rightCharacter="quick"
Вы обманули барышню. На самом деле Вы не знали вернулся Тихон или нет. В отчаянии Вы спустились вниз, в каморку, где обычно слуги дома Курагиных собирались по вечерам и обсуждали новости. Неожиданно Вы там увидели спящего Тихона.
~characterName = yourName
~cloudStatus= 1
Тихон! Тихон!
~characterName = "Тихон"
~cloudStatus= 2
~rightCharacter="Тихон"
Ч-чего тебе, {yourName}.
~characterName = yourName
~cloudStatus= 1
Ты когда приехал? Почему не принёс платье мне? Где платье?
~characterName = ""
~cloudStatus= 0
От Тихона несло лёгким перегаром, теперь Вам стало понятно, где он был и почему не пришёл раньше.
~characterName = "Тихон"
~cloudStatus= 2
П-платье?
~characterName = yourName
~cloudStatus= 1
Да, платье барышни, за которым я тебя посылала.
~characterName = "Тихон"
~cloudStatus= 2
Аааа, п-пл-латье. Оно...оно...оно у К-криллыча...
~characterName = yourName
~cloudStatus= 1
У какого Кириллыча? Ты понимаешь, что и тебе и мне влетит, если сейчас же его не будет.
~characterName = "Тихон"
~cloudStatus= 2
У м-меня!
~characterName = yourName
~cloudStatus= 1
Так у тебя или Кириллыча?
~characterName = "Тихон"
~cloudStatus= 2
Я и есь, ёк, К-криллыч.
~characterName = yourName
~cloudStatus= 1
Хватить тут комедию ломать, где платье?
~characterName = "Тихон"
~cloudStatus= 2
Вон-на ст-туле.
~characterName = yourName
~cloudStatus= 1
Тихон! Как бы дала я тебе и твоему Кириллычу!
~characterName = ""
~cloudStatus= 0
~background="покои"
~rightCharacter="quick"
Вы молниеносно поднялись в комнату Элен.
~characterName = "Элен"
~rightCharacter="Элен"
~cloudStatus= 2
Ну что, ты принесла моё платье?
~characterName = yourName
~cloudStatus= 1
Да, Елена Васильевна!
~characterName = "Элен"
~cloudStatus= 2
Сколько раз я тебя просила меня не называть так. Я Элен!
~characterName = yourName
~cloudStatus= 1
Точно, Элен Васильевна!
~characterName = "Элен"
~cloudStatus= 2
Ох, ладно. Давай платье.
~characterName = ""
~cloudStatus= 0
~rightCharacter="Элен2"
Вы отдали платье, через несколько мгновений оно уже струилось по фигуре Элен Курагиной. Какой же красивой она была. Лицо, белые плечи, грудь, спина — вся она была похожа на древнегреческую богиню. Именно ради такой красоты мужчины развязывают войны.
~characterName = "Элен"
~cloudStatus= 2
Прекрасно! Magnifique!
~leftCharacter="Князь"
~characterName = ""
~cloudStatus= 0
В комнате появился князь Василий.
~cloudStatus= 1
~characterName = "Кн. Василий"

И в кого же ты такая красивая, моя Елена Прекрасная?!
~characterName = "Элен"
~cloudStatus= 2
Papa! Мы уже можем ехать?
~characterName = "Кн. Василий"
~cloudStatus= 1
Да, карета уже готова. Пойдём дорогая.
~characterName = "Элен"
~cloudStatus= 2
Хорошо. Papa, могу я взять с собой нашу служанку? Мне нужна будет её помощь, когда мы приедем к Анне Павловне?
~characterName = "Кн. Василий"
~cloudStatus= 1
Если ты этого хочешь, то, конечно.
~characterName = "Элен"
~cloudStatus= 2
{yourName}, поедешь со мной.
~characterName = yourName
~cloudStatus= 1
~leftCharacter = "Служанка"
Я?
~characterName = "Элен"
~cloudStatus= 2
~music = "карета"
Да, ты. Быстрее пойдём, или я возьму кого-нибудь другого.
~rightCharacter="quick"
~cloudStatus= 0
~characterName = ""
~background="каретаВнутри"
Вы сели в карету и отправились в дом Анны Шерер. Вы не могли поверить, что Вы увидите её, светскую львицу, фрейлину императрицы Марии Федоровны, а может сегодня Вам улыбнётся удача и Вы сможете увидеть и саму царицу. Мысли о том, как худший день превратился в самый лучший не покидали Вас
~characterName = "Кн. Василий"
~cloudStatus= 2
~leftCharacter = "quick"
~rightCharacter="Князь"
~music = "микс"
Вот и приехали. Пойдёмте.
~rightCharacter="quick"
~cloudStatus= 0
~characterName = ""
~background="домВнутри"
Дом Анны Шерер Вам показался огромным. Всё в нём Вам представлялось необычайным, и  даже самые простые вещи для Вас были наполнены невероятной красотой.
~rightCharacter = "Элен2"
~leftCharacter = "Служанка"
~characterName = "Элен"
~cloudStatus= 2
Ну, чего ты рот разинула? Помоги мне привести себя в порядок после дороги.
~characterName = ""
~cloudStatus= 0
Вы помогли Элен поправить платье и причёску.
~characterName = "Элен"
~cloudStatus= 2
Всё, ты свободна, иди!
~characterName = yourName
~cloudStatus= 1
А куда?
~characterName = "Элен"
~cloudStatus= 2
Куда хочешь, можешь пойти домой или подождать нас в карете.
~characterName = yourName
~cloudStatus= 1
Я подожду в карете.
~characterName = "Элен"
~cloudStatus= 2
Иди. Мне нужно немного времени, чтобы настроится.
~characterName = ""
~cloudStatus= 0
Дом Анны Шерер манил Вас. Казалось, что он зазывал Вас остаться здесь навсегда. Но пора было уходить.
~background="сени"
~characterName = ""
~cloudStatus= 0
~rightCharacter="quick"
Неожиданно в сенях Вы встретили свою подругу Марфу.
~characterName = "Марфа"
~rightCharacter = "Марфа"
~cloudStatus= 2
 {yourName}, ты ли это!
 ~characterName = yourName
~cloudStatus= 1
 Марфуша! Как я рада тебя видеть! Что ты здесь делаешь?
 ~characterName = "Марфа"
~cloudStatus= 2
 Я прислуживаю у Анны Павловны, а тебя какими судьбами сюда занесло?
 ~characterName = yourName
~cloudStatus= 1
 Я приехала со своими господами, помочь им подготовится к выходу.
~characterName = yourName
~cloudStatus= 1
 Как же тебе повезло работать в таком доме!
 ~characterName = "Марфа"
~cloudStatus= 2
 Хочешь, я тебя познакомлю с другими слугами?
     +[Да, конечно.]
                        -> Meeting
     +[Нет, моя госпожа велела мне идти в карету.]
                        -> No
=== No === 
~characterName = "Марфа"
~cloudStatus= 2
Ну раз велела, то иди. Не буду тебя задерживать.
  +[Хотя, Марфушка, пять минут ничего не изменят.]
                        -> Meeting
     +[Да, я и так сегодня провинилась. ]
                        -> No2
=== No2 ===
~cloudStatus= 1
~characterName = yourName
Не хочу, чтобы княжна гневалась на меня. Прощай, Марфа! Надеюсь, что ещё свидимся.
~characterName = ""
~cloudStatus= 0
~background="карета"
~rightCharacter="quick"
~leftCharacter ="quick"
~music = "карета"
Вам было грустно покидать этот величественный дом, но Вас радовала мысль, что Вы ещё можете понаблюдать за пребывающими гостями. 
~leftCharacter="Служанка"
~cloudStatus= 2
~characterName = "Кузьмич"
~rightCharacter="Кузьмич"
Ну что, выгнали, как шавку? 
~cloudStatus= 1
~characterName = yourName
Не выгнали, а попросили обождать в карете.
~cloudStatus= 2
~characterName = "Кузьмич"
Знаем, знаем... Ну садись коль сказали обождать
~characterName = ""
~cloudStatus= 0
~rightCharacter="quick"
~background="каретаВнутри"
Вы сели у окна. Через пару минут к крыльцу подъехала ещё одна карета. 
~characterName = ""
~cloudStatus= 0
~background="параКарета"
~leftCharacter="quick"
Из неё вышла пара, верно, муж с женой. Она — такая маленькая и хрупкая, он — высокий и сильный, и... холодный. Он не был груб к своей прекрасной женушке, но чувствовалась отстранённость, и, казалось, что дворецкий был больше рад увидеть маленькую княжну, чем её муж.
~characterName = ""
~cloudStatus= 0
Они вошли в дом и скрылись за массивными красивыми дверьми.
~characterName = ""
~cloudStatus= 0
~background="кареты"
Далее последовала ещё одна карета, потом вторая, третья. Были и другие дамы и господа.
~characterName = ""
~cloudStatus= 0
Все заходили по разному: кто-то прихрамывал, кто-то чуть не упал, кто-то пританцовывал. Но в Вашей памяти почему-то осталась только эта первая пара. Вас мучал вопрос — что же случилось с ним?
~characterName = ""
~cloudStatus= 0
~background="дверь"
~leftCharacter="Князь"
~rightCharacter="Элен2"
В какой-то момент Ваши мысли были прерваны. Вы увидели, что двери открылись и на пороге стояли князь Василий и Елена Васильевна. Рядом с ними появилась какая-то женщина. 
~characterName = ""
~cloudStatus= 0
Её лицо выражала много противоречивых эмоций: злость, смирение, безысходность, надежду.  Казалось, что она и плакала, и ругалась одновременно, при этом стараясь, сохранять правила приличия. Василий Сергеевич пытался отстраниться от всего что происходило, но в глазах было видно, что его что-то терзает изнутри. 
~characterName = ""
~cloudStatus= 0
~background="карета"
~rightCharacter="quick"
Подходя к карете, он небрежно сказал:
~characterName = "Кн. Василий"
~cloudStatus= 1
Кузьмич, трогай домой!
-> END

=== Meeting === 
~characterName = "Марфа"
~cloudStatus= 2
Отлично! Пойдём, узнаем, что у них там происходит.
~cloudStatus= 1
~characterName = yourName
А как мы это узнаем?
~characterName = "Марфа"
~cloudStatus= 2
Ты что не знаешь, что у стен есть свои глаза и уши?
~characterName = ""
~cloudStatus= 0
~music="прислуга"
Вы отправились на кухню.

 ~characterName = "Марфа"
~cloudStatus= 2
~background="кухня"
Ну вот знакомься — это Агафья, наш повар, это Прокопий, кучер, а это Матрёна. Она та нам все новости и расскажет.
~cloudStatus= 1
~characterName = yourName
Очень рада Вас всех увидеть! Я, {yourName}.
~cloudStatus= 2
~characterName =  "Агафья"
~rightCharacter = "Агафья"
Ну ты прям как барышня! «очень рада вас видеть»! Все свои, не надо так выкручиваться.
~cloudStatus= 1
~characterName = yourName
Я и не выкручиваюсь, я такая сама по себе.
~cloudStatus= 2
~characterName =  "Агафья"
Ух, видишь ли!..
~cloudStatus= 1
~characterName =  "Прокофий"
~leftCharacter = "Прокофий"
Агафья, да будет тебе. Только познакомились, а ты уже девку давишь!
~cloudStatus= 2
~characterName =  "Агафья"
Прошу прощения! Так ведь Ваши господы разговаривают?
~cloudStatus= 1
~characterName =  "Прокофий"
Агафья!
~cloudStatus= 2
~characterName =  "Агафья"
Ладно, ладно... Прости, {yourName}. Не люблю я, когда люди не настоящие...
  ~characterName = "Марфа"
~cloudStatus= 2
~rightCharacter="Марфа"
~leftCharacter = ""
Давайте лучше Матрёну послушаем, ну что там?
~cloudStatus= 1
~characterName =  "Матрёна"
~leftCharacter = "Матрёна"
~rightCharacter="quick"
Пока не знаю. Я всё расставляла ещё до прихода гостей. Сейчас, опять пойду и тогда уже расскажу.
~characterName = ""
~cloudStatus= 0
~leftCharacter="quick"
Матрёна покинула кухню с несколькими подносами. Через какое-то время она вернулась. Все присутствующие обратили на неё взоры, желая услышать, кто сегодня танцует и веселится в главной зале.
~leftCharacter = "Матрёна"
~cloudStatus= 1
~characterName =  "Матрёна"
Ох, ребятушки, не поверите, кого я там видела!
~cloudStatus= 2
~characterName =  "Агафья"
~rightCharacter="Агафья"
Не уж то тсарицу! 
~cloudStatus= 1
~characterName =  "Матрёна"
~rightCharacter="quick"
Нет, не царицу. Прибыл виконт М... А он принадлежит к одной из лучших фамилий самой ФранцИи. Анна Павловна так сказала, сама слышала. 
~cloudStatus= 1
~characterName =  "Матрёна"
Такие он истории занимательные рассказывает. Что-то на французском, но так красиво, что слушать его можно без понимания. 
~cloudStatus= 1
~characterName =  "Матрёна"
Прибыли и Болконские. Лизавета нежная прекрасная, сидит вышивает что-то, а муж её какой-то слишком грозный.
~cloudStatus= 1
~characterName =  "Матрёна"
Пьер Безухов приехал.
~cloudStatus= 2
~rightCharacter="Агафья"
~characterName =  "Агафья"
А эт ещё кто фтакой? 
~cloudStatus= 1
~characterName = yourName
Это один из дальних родственников моих господ. Пьер Кириллович похож на большого ребёнка: он наивен, добр, естественен. 
~cloudStatus= 2
~characterName =  "Агафья"
А выглядит как?
~cloudStatus= 1
~characterName = yourName
Массивен, толст, лыс.
~characterName = ""
~cloudStatus= 0
~rightCharacter="quick"
~leftCharacter = "Домоуправительница"
С этими словами в кухню вошла домоуправительница.
~cloudStatus= 1
~characterName =  "Домоуправительница"
А что это мы тут на кухне сидим, прохлаждаемся? А ну быстро за работу!
~cloudStatus= 1
~characterName =  "Домоуправительница"
~rightCharacter="Служанка"
А ты ещё кто такая?
~cloudStatus= 2
~characterName = yourName
Я... я служанка из дома Курагиных.
~cloudStatus= 1
~characterName =  "Домоуправительница"
~music=""
Ты, служанка, дома что ли попутала, выметайся отсюда. Немедленно!
~leftCharacter="quick"
~background="лестница2"
~cloudStatus = 0
~characterName = ""
~music = "карета"
Спускаясь по лестнице к карете, Вы обернулись ещё раз полюбоваться роскошью и богатством этого дома.
~leftCharacter="Кузьмич"
~background="карета"
~cloudStatus= 1
~characterName =  "Кузьмич"
Что домой поедем или господ подождём?
  +[Домой.]
                        -> Home
     +[Подождём.]
                        -> Wait

=== Home ===
~characterName = ""
~cloudStatus= 0
~leftCharacter="quick"
~rightCharacter = "quick"
И вы быстро помчались по узким улочками к своему дому, к дому князя Курагина.
->END
=== Wait ===
~characterName = ""
~cloudStatus= 0
~leftCharacter="quick"
~background="каретаВнутри"
Вы сели в карету и наблюдали за пребывающими людьми. В своих мыслях, Вы представляли, что и Вас привезли на вечер и через несколько минут Вы снова войдёте в этот блистающий дом. 
->END
=== B ===
~characterName = "Элен"
~cloudStatus= 2
Как я тебе не говорила? Да, даже если я тебе не сказала, ты сама должна была об этом догадаться! Вот бестолочь! 
~characterName = yourName
~cloudStatus= 1
Но, Елена Васильевна...
~characterName = "Элен"
~cloudStatus= 2
Ничего не желаю слышать! Или через час на мне будет это платье или ищи себе другое место!
~characterName = yourName
~cloudStatus= 1
Но ателье уже, скорее всего, закрыто.
~characterName = "Элен"
~cloudStatus= 2
Слышать ни-че-го не хо-чу!
~characterName = "Элен"
~cloudStatus= 2
Позови Фёклу, пусть она собирёт меня, а ты пока доставай платье, как хочешь.
->Sad
-->END

=== C ===
~characterName = "Элен"
~cloudStatus= 2
Конечно!
~characterName = yourName
~cloudStatus= 1
А как оно выглядит?
~characterName = "Элен"
~cloudStatus= 2
Ты что издеваешься надо мной? Моё прекрасное нежно-голубое платье, которое мы заказывали в ателье у швеи N. Ты что ли головой ударилась, что всё забыла?
  +[Промолчать]
                        -> C1
     +[Да, в последнее время не важно себя чувствую... ]
                        -> C1
-->END

=== C1 === 
~characterName = "Элен"
~cloudStatus= 2
Ну так что? Где моё платье?
~characterName=""
~cloudStatus= 0
Вы не представляли о каком платье идёт речь, ведь голубых у княжны было достаточно, а у швеи N она заказывала платья чуть ли не каждую неделю, но ясно было одно — платья в доме нет. Нужно было что-то придумать.
  +[Я отдала его Дуняше на сохранение.]
                        ->Dynya
     +[Его Настасья постирала. Сейчас гладит.]
                        -> Nastya
        +[Не буду врать, барышня.  Я забыла его забрать из ателье.]
                        ->Angry_Elen         
=== Dynya ===
~characterName = "Элен"
~cloudStatus= 2
Дуняше? Моё платье? Неси его скорее!
~characterName = yourName
~cloudStatus= 1
Боюсь, что только через час смогу его принести, ведь Дуня уехала по делам, а где оно хранится, я не знаю.
~characterName = "Элен"
~cloudStatus= 2
Ох, Дуняша! Она у меня ещё пожалеет!
~characterName = "Элен"
~cloudStatus= 2
Если через час платья у меня не будет, то и ты и твоя Дуняша вылетят из этого дома!
-> two_variants
=== Nastya ===
~characterName = "Элен"
~cloudStatus= 2
Как постирала?! Зачем?!
~characterName = yourName
~cloudStatus= 1
Когда Тихон его забирал, края платья немного запачкались, вот и решили мы его постирать.
~characterName = "Элен"
~cloudStatus= 2
Какой кошмар! Надеюсь, что мои блёсточки и рюшечки не отвалились, иначе это будет позор. Что скажут люди? «Княжна Курагина пришла на вечер в ... в... простыне».
~characterName = ""
~cloudStatus= 0
Слёзы нахлынули на Элен. 

~characterName = yourName
~cloudStatus= 1
Не беспокойтесь, барышня. Всё будет хорошо с Вашим платьем.
->two_variants
=== two_variants  ===
~characterName = ""
~cloudStatus= 0
~rightCharacter="quick"
~background="коридор"
Вы вышли из комнаты и думали, что же Вам сделать. Варианта было два: бежать в ателье самой или послать Тихона.
  +[Бежать в ателье]
                        -> atelye
     +[Послать Тихона в ателье]
 
                       -> Tyhon
=== atelye === 
~characterName = ""
~cloudStatus= 0
Было решено идти в ателье самой. Вы быстро собрались и уже хотели выйти из дома, как вдруг неожиданно услышали голос князя Василия. Он звал Вас к себе.
-> Звали

=== Tyhon ===
~characterName = ""
~cloudStatus= 0
Идти в ателье пешком было далеко, поэтому Вы решили отправить Тихона за платьем. Дали наставление и деньги, чтобы он заплатил за работу. А пока решили вновь подняться к молодой княжне.
~background="покои"
~rightCharacter="Элен"
~characterName="Элен"
~cloudStatus = 2
{yourName}, начисти мне туфли и принеси украшения с бабочками. Думаю, может надеть их... Ах, да и Фёклу позови. Пусть она мне сделает причёску.
~characterName = yourName
~cloudStatus= 1
Как скажете, барышня.
->Poklon