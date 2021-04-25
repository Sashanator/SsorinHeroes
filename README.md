# SsorinHeroes

## [ENG]
## Overview

This educational project is a **game** with a graphical interface, vaguely reminiscent of the well-known series of computer games in the turn-based strategy genre - Heroes of Might and Magic. SsorinHeroes is developed on the WPF platform without additional graphics libraries, all styles were written manually using XAML.
**Gameplay** is a turn-based battle on a map, 10 by 10 cells. You play with 3 characters (standard settings, can be changed in the main menu), each of which has a number of characteristics that affect the course of the battle. Your goal for victory is to defeat all dragons on the battlefield. You can move your units and attack enemy monsters. Each friendly and enemy character (except for the witch) has the following characteristics:
- Leadership (affects the order of moves)
- Action points (required for an attack, replenished over time)
- Health (when it drops to zero, the character dies and disappears from the battlefield)
- Attack (amount of damage done)
- Protection (the number by which the damage done to you is reduced)
- Move (the number of cells that your character can go to)
- Luck (chance to inflict critical damage on the enemy)
- Accuracy (chance to inflict damage on the enemy)

Dragons in this game are not the smartest, so the **artificial intelligence** of opponents follows these instructions: it moves to the nearest enemy (i.e. one of your characters) and as soon as it gets close enough to attack, it fights to the death.

On the battlefield, you are assisted by another character - a witch, for whom artificial intelligence plays. Depending on the difficulty level, it will move once every a certain number of moves (for example, at the last difficulty level, it will only move once every 20 moves). Important: if all your heroes died, then the witch will not continue to fight for you and will leave the battlefield, and you will be considered defeated!
The witch is very different in nature from friendly and enemy characters. Firstly, she is the only one on the entire battlefield that has the ability to attack with a ranged spell (while she has a sword, which she skillfully wields in close combat). Secondly, it has increased characteristics that differ from ordinary units (for example, it has no action points, that is, it can hit endlessly). The witch has the following characteristics:
- Leadership (affects the order of moves)
- Mana (spent on spells)
- Health (when it drops to zero, the character dies and disappears from the battlefield)
- Attack (amount of damage dealt in MELEE battle)
- Protection (the number by which the damage done to you is reduced)
- Movement (number of cells that the sorceress can go to)
- Luck (chance to inflict critical damage on the enemy)
- Accuracy (chance to inflict damage on the enemy)

Her spell - "Fireball" also has its own characteristics, scaling from the level of difficulty:
- Damage (how much the spell will injure)
- Mana (spell cost)
- Error (chance of a magical error, in which the expended forces were wasted and the spell did not work)
- Distance (distance, measured in the number of cells to the target)

The AI of the witch is no longer as easy as the AI of the dragons. She focuses on an incomparable advantage in ranged combat, so she tries to keep her distance from the enemy as long as she has the ability to bombard them with spells. As soon as she runs out of mana, she will go into battle with a sword on the nearest weakened enemy.


## Features

- "Unrivaled" artificial intelligence
- Graphic design in a minimalist style
- Flexible adjustment of the difficulty level
- A large number of characteristics affecting the course of the battle
- The game is written in .NET Framework 4.6.1 (Windows only)

## Usage
When you start the game, you will see the difficulty settings window, where you can:
- choose the number of enemy units
- choose the level of enemy units (on which their characteristics depend)
- choose the number of friendly units
- choose the level of friendly units (on which their characteristics depend)
- choose the general level of difficulty, on which some aspects of the game depend

Screenshot of difficulty settings:
![OptionMenu](https://github.com/Sashanator/SsorinHeroes/blob/main/SsorinHeroes/img/gameMenu.png)

After clicking on the "Play!" You will be taken to the playing field. From this moment, the "Initial phase of the battle" begins, in which you can correct the initial position of all your characters (except for the witch) and study their characteristics. Upon completion of all actions, click the "End move" button, the system notifies when an enemy monster or witch is walking. After that, all your characters take turns, depending on their Leadership attribute. All characters fight in close combat (except for the witch), therefore, in order to hit the enemy, you need to come close to him vertically or horizontally.

Gameplay:
![Gameplay](https://github.com/Sashanator/SsorinHeroes/blob/main/SsorinHeroes/img/sample_1.gif)

## Author
- **Alexander Ssorin** – game developer – [Sashanator](https://github.com/Sashanator)

## Licence
- [MIT](https://choosealicense.com/licenses/mit/)

## [RUS]
## Обзор

Данный учебный проект представляет собой **игру** с графическим интерфейсом, отдалённо напоминающую известную серию компьютерных игр в жанре пошаговой стратегии – Heroes of Might and Magic (Герои меча и магии). SsorinHeroes разработана на платформе WPF без дополнительных графических библиотек, все стили были прописаны вручную с помощью XAML. 
**Игровой процесс** представляет из себя пошаговый бой на карте, размером 10 на 10 клеток. Вы играете 3-мя персонажами (стандартные настройки, можно изменить в главном меню), каждый из которых имеет целый ряд характеристик, влияющих на ход боя.  Ваша цель для победы: победить всех драконов на поле боя. Вы можете передвигать своих юнитов и атаковать вражеских монстров. Каждый дружеский и вражеский персонаж (кроме чародейки) обладает следующими характеристиками:
- Лидерство (влияет на очерёдность ходов)
- Очки действий (требуются для атаки, восполняются со временем)
- Здоровье (когда оно опустится до нуля, персонаж погибает и исчезает с поля боя)
- Атака (кол-во наносимого урона)
- Защита (кол-во, на которое уменьшается наносимый вам урон)
- Перемещение (кол-во клеток, на которые ваш персонаж может пройти)
- Удача (шанс нанести противнику критический урон)
- Точность (шанс нанести противнику урон)

Драконы в данной игре не самые умные, поэтому **искусственный интеллект** противников следует данным инструкциям: он передвигается к ближайшему врагу (т.е. одному из ваших персонажей) и как только подойдёт достаточно близко для атаки – дерётся до смерти.

На поле боя Вам помогает ещё один персонаж – чародейка, за который играет искусственный интеллект. В зависимости от уровня сложности, она будет ходить раз в какое-то количество ходов (например, на последнем уровне сложности она будет ходить лишь раз в 20 ходов). Важно: если все ваши герои погибли, то чародейка не будет и дальше за Вас сражаться и покинет поле боя, а вам будет засчитано поражение!
Чародейка по своей сущности сильно отличается от дружеских и вражеских персонажей. Во-первых, она единственная на всём поле боя имеет возможность дальнобойной атаки с помощью заклинания (при этом у неё есть меч, которым она умело орудует и в ближнем бою). Во-вторых, она имеет повышенные характеристики, которые отличаются от обычных юнитов (например, у неё нет очков действий, то есть она может бить бесконечно). Чародейка имеет следующие характеристики:
- Лидерство (влияет на очерёдность ходов)
- Мана (расходуется на заклинания)
- Здоровье (когда оно опустится до нуля, персонаж погибает и исчезает с поля боя)
- Атака (кол-во наносимого урона в БЛИЖНЕМ бою)
- Защита (кол-во, на которое уменьшается наносимый вам урон)
- Перемещение (кол-во клеток, на которые чародейка может пройти)
- Удача (шанс нанести противнику критический урон)
- Точность (шанс нанести противнику урон)

Её заклинание – "Огненный шар" также имеет свои характеристики, скалирующиеся от уровеня сложности:
- Урон (сколько заклинание нанесёт увечий)
- Мана (стоимость заклинания)
- Ошибка (шанс магической ошибки, при которой затраченные силы были использованы впустую и заклинание не сработало)
- Дистанция (дистанция, измеряемся в количестве клеток до цели)

ИИ чародейки работает уже не так просто, как ИИ драконов. Она делает упор на несравнимое преимущество в дальнем бою, поэтому старается держать дистанцию с противником до тех пор, пока она имеет возможность обстреливать их заклинаниями. Как только у неё кончится мана, она пойдёт в бой с мечом на ближайшего ослабевшего врага. 


## Особенности игры

- "Непривзойдённый" искусственный ителлект
- Графическое оформление в минималистичном стиле
- Гибкая настройка уровня сложности
- Большое количество характеристик влияющих на ход боя
- Игра написана на .NET Framework 4.6.1 (Windows only)

## Использование
При запуске игры Вы увидите окно настроек сложности, где Вы можете:
- выбрать количество вражеских юнитов
- выбрать уровень вражеских юнитов (от которых зависят их характеристики)
- выбрать количество дружеских юнитов
- выбрать уровень дружеских юнитов (от которых зависят их характеристики)
- выбрать общий уровень сложности, от которого зависят некоторые аспекты игры

Скриншот настроек сложности:
![OptionMenu](https://github.com/Sashanator/SsorinHeroes/blob/main/SsorinHeroes/img/gameMenu.png)

После нажатия на кнопку "Играть!" Вы переместитесь на игровое поле. С этого момента начинается "Начальная фаза боя", на которой Вы можете подкорректировать начальное положение всех ваших персонажей (кроме чародейки) и изучить их характеристики. По завершению всех действий, нажмите кнопку "Завершить ход", система оповещает о том, когда ходит вражеский монстр или чародейка. После этого, все ваши персонажи ходят по очереди, в зависимости от их атрибута "Лидерство". Все персонажи сражаются в ближнем бою (кроме чародейки), поэтому, чтобы ударить противника, нужно подойти к нему вплотную вертикально или горизонтально. 

Игровой процесс:
![Gameplay](https://github.com/Sashanator/SsorinHeroes/blob/main/SsorinHeroes/img/sample_1.gif)

## Автор
- **Александр Ссорин** – разработчик игры – [Sashanator](https://github.com/Sashanator)

## Лицензия
- [MIT](https://choosealicense.com/licenses/mit/)