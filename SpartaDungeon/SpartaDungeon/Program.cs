using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using static SpartaDungeon.Program;

namespace SpartaDungeon
{
    public class ReturnToRobbyException : Exception
    {
        public ReturnToRobbyException() { }
    }

    internal class Program
    {
        //플레이어 클래스
        public class Player
        {
            public string? PlayerName { get; set; }
            public string? PlayerClass { get; set; }
            public int PlayerLevel { get; set; }
            public int PlayerAtt { get; set; }
            public int PlayerDfn { get; set; }
            public int PlayerMaxHP { get; set; }
            public int PlayerHP { get; set; }
            public int PlayerGold { get; set; }

            //장착한 장비
            public Weapon? EquippedWeapon { get; set; }
            public Armor? EquippedArmor { get; set; }

            //데미지 메서드
            public void GetDamage(int damage)
            {
                player.PlayerHP -= damage;
                if (player.PlayerHP <= 0)
                {
                    player.PlayerHP = 1;
                    player.PlayerGold /= 2;
                    Console.WriteLine($"{player.PlayerName}의 기력이 다했습니다.");
                    Console.WriteLine($"돈을 일부 잃고 마을로 돌아옵니다.");
                    return;
                }
                else
                {
                    Console.WriteLine($"{damage}의 데미지를 입었습니다.");
                }

            }
        }

        //직업 클래스
        public class Job
        {
            public string Name { get; }
            public int Att { get; }
            public int Dfn { get; }
            public int HP { get; }
            public int StartGold { get; }

            public Job(string name, int att, int dfn, int hp, int gold)
            {
                Name = name;
                Att = att;
                Dfn = dfn;
                HP = hp;
                StartGold = gold;
            }
        }

        //아이템 클래스
        public class Item
        {
            public string? ItemName { get; set; }
            public string? ItemTooltip { get; set; }
            public int ItemAtt { get; set; }
            public int ItemDfn { get; set; }
            public int Count { get; set; }
            public int Tier { get; set; }
            public bool Equip { get; set; }
            public string? Type { get; set; }
        };


        //아이템 - 무기 클래스
        public class Weapon : Item
        {
            public Weapon(string name, string tooltip, int att, int tier)
            {
                ItemName = name;
                ItemTooltip = tooltip;
                ItemAtt = att;
                Count = 0;
                Tier = tier;
                Equip = false;
                Type = "Weapon";
            }

            public Weapon() { }
        }


        //아이템 - 방어구 클래스
        public class Armor : Item
        {
            public Armor(string name, string tooltip, int dfn, int tier)
            {
                ItemName = name;
                ItemTooltip = tooltip;
                ItemDfn = dfn;
                Count = 0;
                Tier = tier;
                Equip = false;
                Type = "Armor";
            }

            public Armor() { }
        }

        //플레이어 생성
        static Player player = new Player();

        //직업 생성
        //다른 직업 미구현
        static List<Job> jobList = new List<Job>
        {
            new Job("모험가", 10, 5, 100, 10000)
        };

        //아이템 생성
        static public List<Item> ItemList = new List<Item>
            {
                new Weapon("막대기", "산에서 흔히 주울 수 있는 막대기. 무기인가?", 10, 1),
                new Weapon("몽둥이", "길고 큰 몽둥이. 여러 선생님들의 최애템이다.", 20, 2),
                new Weapon("오래된 칼", "오래되어 부서지기 직전의 칼. 잠깐은 쓸만한거 같다.", 35, 3),
                new Weapon("무딘 칼", "오래되어 무뎌진 칼. 둔기에 가깝다.", 55, 4),
                new Weapon("철 검", "평범한 철검. 모험을 시작할 때 아주 좋은 무기다.", 70, 5),
                new Weapon("잘 벼려진 칼", "아주 날카로운 칼. 닿기만 해도 베인다.", 100, 6),
                new Weapon("장인의 검", "고명한 장인이 손수 만든 검. 아주 튼튼하고 잘 베인다.", 150, 7),
                new Weapon("마법 검", "마법이 인챈트된 검. 이 검만 있으면 일당백이 가능하다.", 300, 8),
                new Weapon("명검", "세계에 몇자루 없다는 명검이다. 산을 가르고 바다를 가른다는 소문이 있다.", 500, 9),
                new Weapon("전서래검", "선택받은 자만이 들 수 있는 검이다. 전설의 검과는 다르다.", 1000, 10),

                new Armor("누더기", "오래 입어 다 해진 누더기. 입으면 초라해진 자신을 볼 수 있다", 5, 1),
                new Armor("헌 옷", "낡고 헤진 옷. 방어력은 없다싶이 하다.", 7, 2),
                new Armor("두꺼운 옷", "여러 옷을 겹친 옷이다. 덥다.", 10, 3),
                new Armor("천 갑옷", "그냥 옷보다는 방어력이 좋다. 통기성도 좋다", 15, 4),
                new Armor("가죽 갑옷", "평범한 가죽 갑옷. 모험을 시작할 때 좋은 방어구다.", 25, 5),
                new Armor("철 갑옷", "튼튼한 갑옷. 적의 공격을 잘 막아준다.", 50, 6),
                new Armor("튼튼한 갑옷","이상하게 튼튼한 갑옷. 한평생을 써도 부서지지 않을 것이다.", 100, 7),
                new Armor("마법 갑옷","마법이 인챈트된 갑옷. 용의 공격도 막아준다고 한다.", 200, 8),
                new Armor("용비늘 갑옷","명검과 버금가는 갑옷. 이 갑옷을 입고 막지 못할 공격은 없다.", 350, 9),
                new Armor("전서래가봇","선택받은 자만이 입을 수 있는 갑옷. 전설의 갑옷과는 다르다.", 500, 10)
            };

        //마지막에 들어간 던전
        static public int LastDungeon;

        //세이브 데이터 클래스
        class SaveData
        {
            public Player? Player { get; set; }
            public List<Item>? Item { get; set; }
            public int LastDungeon { get; set; }
        }


        static void Main(string[] args)
        {
            //선택지 배열
            int[] choices = { 1, 2 };
            Console.WriteLine("1. 새로하기");
            Console.WriteLine("2. 불러오기");

            //Input 메서드에서 입력이 제대로 됐는지 확인
            int choice = Input(choices);

            switch (choice)
            {
                case 1:
                    MakeChar(); //1일때 캐릭터 생성
                    break;
                case 2:
                    while (true)
                    {
                        bool success = LoadGame(); //로드

                        if (success)
                        {
                            Robby(); //로드 성공시 로비로
                            break;
                        }
                        else
                        {
                            Console.WriteLine("불러오기에 실패했습니다"); //실패시 다시시작
                            Main(args);
                        }
                    }
                    break;
            }
        }

        static void MakeChar()
        {
            while (true)
            {
                //이름, 직업 선택
                MakeName();
                MakeClass();
                //선택 후 확인 작업
                bool confirmed = ConfrimChar();

                if (confirmed)
                {
                    Robby();
                    break;
                }
            }
        }

        static void MakeName()
        {
            Console.WriteLine("이름을 입력해 주세요.");
            Console.WriteLine();
            Console.Write(">>");
            player.PlayerName = Console.ReadLine();
            while (true)
            {
                if (player.PlayerName == null)
                {
                    Console.WriteLine("다시 입력해 주세요.");
                    Console.WriteLine();
                    Console.Write(">>");
                }
                else
                {
                    return;
                }
            }
        }

        static void MakeClass()
        {
            int[] choices = Enumerable.Range(0, jobList.Count + 1).ToArray();

            Console.WriteLine();
            Console.WriteLine("직업을 선택해 주세요.");
            Console.WriteLine();
            for (int i = 0; i < jobList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {jobList[i].Name}");
            }
            int choice = Input(choices);
            Job selectedJob = jobList[choice - 1];

            //생성된 플레이어에 직업 스탯 대입
            player.PlayerClass = selectedJob.Name;
            player.PlayerLevel = 1;
            player.PlayerAtt = selectedJob.Att;
            player.PlayerDfn = selectedJob.Dfn;
            player.PlayerMaxHP = selectedJob.HP;
            player.PlayerHP = selectedJob.HP;
            player.PlayerGold = selectedJob.StartGold;
        }

        static bool ConfrimChar()
        {
            int[] choices = { 1, 2 };

            Console.WriteLine($"이름 : {player.PlayerName}");
            Console.WriteLine($"직업 : {player.PlayerClass}");
            Console.WriteLine("으로 틀림없습니까");
            Console.WriteLine();
            Console.WriteLine("1. 결정");
            Console.WriteLine("2. 취소");

            int choice = Input(choices);
            if (choice == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Robby()
        {
            //메서드 마다 반복이 잦아 스택에 쌓이는거 방지
            while (true)
            {
                int[] choices = { 1, 2, 3, 4, 5, 6 };

                Console.WriteLine("마을에 오신 여러분 환영합니다");
                Console.WriteLine("이곳에서 던전으로 들어가기전 행동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 여관");
                Console.WriteLine("5. 던전");
                Console.WriteLine("6. 저장하기");
                Console.WriteLine();

                int choice = Input(choices);

                switch (choice)
                {
                    case 1:
                        State();
                        break;
                    case 2:
                        Inventory();
                        break;
                    case 3:
                        Store();
                        break;
                    case 4:
                        Inn();
                        break;
                    case 5:
                        Dungeon();
                        break;
                    case 6:
                        SaveGame();
                        break;
                }
            }
        }

        static void State()
        {
            while (true)
            {
                int[] choices = { 0, 1 };

                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표기됩니다.");
                Console.WriteLine();
                Console.WriteLine($"Lv. {player.PlayerLevel}");
                Console.WriteLine($"{player.PlayerName} ( {player.PlayerClass} )");
                Console.WriteLine($"생명력 : {player.PlayerHP} / {player.PlayerMaxHP}");
                //아이템에 의한 스탯 변화를 직관적으로 표시
                //아이템이 null인 경우 0으로 표기
                Console.WriteLine($"공격력 : {player.PlayerAtt} + {player.EquippedWeapon?.ItemAtt ?? 0}");
                Console.WriteLine($"방어력 : {player.PlayerDfn} + {player.EquippedArmor?.ItemDfn ?? 0}");
                Console.WriteLine($"Gold :  {player.PlayerGold} G");
                Console.WriteLine();
                Console.WriteLine("1. 기초 훈련");
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        LevelUp();
                        break;

                }
            }
        }

        static void LevelUp()
        {
            while (true)
            {
                int[] choices = { 0, 1, 2, 3 };
                int levelUpGold = 1000 * player.PlayerLevel;

                Console.WriteLine("상태보기 - 기초 훈련");
                Console.WriteLine("돈을 지불해 스텟을 올립니다.");
                Console.WriteLine("공격력, 방어력은 1씩, 생명력 10씩 오릅니다.");
                Console.WriteLine();
                Console.WriteLine($"훈련 비용 : {levelUpGold}");
                Console.WriteLine();
                Console.WriteLine($"Gold :  {player.PlayerGold} G");
                Console.WriteLine($"1. 생명력 : {player.PlayerHP} / {player.PlayerMaxHP}");
                Console.WriteLine($"2. 공격력 : {player.PlayerAtt} + {player.EquippedWeapon?.ItemAtt ?? 0}");
                Console.WriteLine($"3. 방어력 : {player.PlayerDfn} + {player.EquippedArmor?.ItemDfn ?? 0}");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        if (player.PlayerGold < levelUpGold)
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                        else
                        {
                            player.PlayerGold -= levelUpGold;
                            player.PlayerMaxHP += 10;
                            player.PlayerLevel++;
                        }
                        break;
                    case 2:
                        if (player.PlayerGold < levelUpGold)
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                        else
                        {
                            player.PlayerGold -= levelUpGold;
                            player.PlayerAtt++;
                            player.PlayerLevel++;
                        }
                        break;
                    case 3:
                        if (player.PlayerGold < levelUpGold)
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                        else
                        {
                            player.PlayerGold -= levelUpGold;
                            player.PlayerDfn++;
                            player.PlayerLevel++;
                        }
                        break;
                }
            }
        }

        static void Inventory()
        {
            while (true)
            {
                int[] choices = { 0, 1, 2 };
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                //아이템 리스트에서 가지고 있는 개수가 1개 이상인 것을 item으로 참조
                foreach (Item item in ItemList.Where(x => x.Count > 0))
                {
                    if (item.Equip == true)
                    {
                        Console.WriteLine($"[E] {item.Tier}Tier | {item.ItemName} | {item.ItemTooltip} | {item.Count}");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Tier}Tier | {item.ItemName} | {item.ItemTooltip} | {item.Count}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("1. 장착하기");
                Console.WriteLine("2. 해제하기");
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        Equipped();
                        break;
                    case 2:
                        UnEquip();
                        break;
                }
            }
        }

        static void Equipped()
        {
            while (true)
            {
                List<Item> item = ItemList.Where(x => x.Count > 0).ToList();
                //개수가 1개 이상인 item리스트의 길이로 선택지를 생성
                //개수를 저장하는 count와 리스트의 길이를 세는 count가 겹침
                //수정 필요
                int[] choices = Enumerable.Range(0, item.Count + 1).ToArray();

                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("장착할 아이템을 선택해 주세요.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < item.Count; i++)
                {
                    if (item[i].Equip == true)
                    {
                        Console.WriteLine($"[E] {i + 1}. {item[i].Tier} | {item[i].ItemName,-10} | 공격력 : +{item[i].ItemAtt} | 방어력 : +{item[i].ItemDfn}");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {item[i].ItemName,-10} | 공격력 : +{item[i].ItemAtt} | 방어력 : +{item[i].ItemDfn}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    default:
                        //i는 1부터 시작 배열은 0부터니 1빼주기
                        int i = choice - 1;
                        //선택된 i는 int이니 Item으로 변환
                        Item choiceItem = item[i];

                        //선택된 아이템이 무기일 경우
                        if (choiceItem is Weapon weapon)
                        {
                            //무기를 장착 중일 때 선택된 무기와 바꿔주기
                            if (player.EquippedWeapon != null)
                            {
                                player.EquippedWeapon.Equip = false;
                                weapon.Equip = true;
                                Console.WriteLine($"{weapon.ItemName} 을(를) 장착했습니다.");
                            }
                            else if (player.EquippedWeapon == weapon)
                            {
                                Console.WriteLine("이미 장착 중인 아이템입니다.");
                            }
                            else
                            {
                                player.EquippedWeapon = weapon;
                                weapon.Equip = true;
                                Console.WriteLine($"{weapon.ItemName} 을(를) 장착했습니다.");
                            }
                        }
                        //선택된 아이템이 방어구일 경우
                        else if (choiceItem is Armor armor)
                        {
                            if (player.EquippedArmor != null)
                            {
                                player.EquippedArmor.Equip = false;
                                armor.Equip = true;
                                Console.WriteLine($"{armor.ItemName} 을(를) 장착했습니다.");
                            }
                            else if (player.EquippedArmor == armor)
                            {
                                Console.WriteLine("이미 장착 중인 아이템입니다.");
                            }
                            else
                            {
                                player.EquippedArmor = armor;
                                armor.Equip = true;
                                Console.WriteLine($"{armor.ItemName} 을(를) 장착했습니다.");
                            }
                        }
                        break;
                }
            }
        }

        static void UnEquip()
        {
            while (true)
            {
                List<Item> item = ItemList.Where(x => x.Count > 0).ToList();
                int[] choices = Enumerable.Range(0, item.Count + 1).ToArray();

                Console.WriteLine("인벤토리 - 장착 해제");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("장착 해제할 아이템을 선택해 주세요.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < item.Count; i++)
                {
                    if (item[i].Equip == true)
                    {
                        Console.WriteLine($"[E] {i + 1}. {item[i].ItemName,-10} | 공격력 : +{item[i].ItemAtt} | 방어력 : +{item[i].ItemDfn}");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {item[i].ItemName,-10} | 공격력 : +{item[i].ItemAtt} | 방어력 : +{item[i].ItemDfn}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    default:
                        int i = choice - 1;
                        Item choiceItem = item[i];

                        if (choiceItem is Weapon weapon)
                        {
                            if (player.EquippedWeapon == weapon)
                            {
                                player.EquippedWeapon = null;
                                weapon.Equip = false;
                                Console.WriteLine($"{weapon.ItemName} 을(를) 장착 해제했습니다.");
                            }
                            else
                            {
                                Console.WriteLine($"{weapon.ItemName} 은(는) 장착 중인 아이템이 아닙니다.");
                            }
                        }
                        else if (choiceItem is Armor armor)
                        {
                            if (player.EquippedArmor == armor)
                            {
                                player.EquippedArmor = null;
                                armor.Equip = false;
                                Console.WriteLine($"{armor.ItemName} 을(를) 장착 해제했습니다.");
                            }
                            else
                            {
                                Console.WriteLine($"{armor.ItemName} 은(는) 장착 중인 아이템이 아닙니다.");
                            }
                        }
                        break;
                }
            }
        }

        static void Store()
        {
            while (true)
            {
                int[] choices = { 0, 1, 2 };

                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.PlayerGold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine();
                Console.WriteLine($"{ItemList[0].ItemName} | {ItemList[0].ItemTooltip} ");
                Console.WriteLine($"{ItemList[10].ItemName} | {ItemList[10].ItemTooltip} ");
                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 강화");
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        StoreBuy();
                        break;
                    case 2:
                        UpGrade();
                        break;
                }
            }
        }

        static void StoreBuy()
        {
            while (true)
            {
                int[] choices = { 0, 1, 2 };

                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("구매 할 아이템을 선택해 주세요.");
                Console.WriteLine();
                Console.WriteLine("주인장 : 뭐 이리 비싸냐고? 내 마음이지! 낄낄");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.PlayerGold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine($"[1] {ItemList[0].Tier}Tier | {ItemList[0].ItemName} | 공격력 : {ItemList[0].ItemAtt} | 10000 G | {ItemList[0].Count}");
                Console.WriteLine($"[2] {ItemList[10].Tier}Tier | {ItemList[10].ItemName} | 방어력 : {ItemList[10].ItemDfn} | 10000 G | {ItemList[10].Count}");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        if (player.PlayerGold >= 10000)
                        {
                            ItemList[0].Count++;
                            player.PlayerGold -= 10000;
                            Console.WriteLine($"{ItemList[0].ItemName}을(를) 구매하셨습니다.");
                        }
                        else
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                        break;
                    case 2:
                        if (player.PlayerGold >= 10000)
                        {
                            ItemList[10].Count++;
                            player.PlayerGold -= 10000;
                            Console.WriteLine($"{ItemList[10].ItemName}을(를) 구매하셨습니다.");
                        }
                        else
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                        break;
                }
            }
        }

        static void UpGrade()
        {
            while (true)
            {
                List<Item> item = ItemList.Where(x => x.Count > 0).ToList();
                int[] choices = Enumerable.Range(0, item.Count + 1).ToArray();

                Console.WriteLine("상점 - 아이템 강화");
                Console.WriteLine("같은 아이템 2개를 사용해 상위 아이템으로 교체합니다.");
                Console.WriteLine();
                Console.WriteLine("주인장 : 그래도 강화는 공짜로 해주잖아? 껄껄");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < item.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {item[i].Tier}Tier | {item[i].ItemName} | 공격력 : +{item[i].ItemAtt} | 방어력 : +{item[i].ItemDfn} | 수량 : {item[i].Count}");
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);
                int j = choice - 1;

                switch (choice)
                {
                    case 0:
                        {
                            Store();
                            return;
                        }
                    default:
                        {
                            //선택된 아이템이 원본 아이템 리스트에 몇번째 인수인지 확인하고 index에 저장
                            int index = ItemList.IndexOf(item[j]);
                            if (item[j].Count >= 2 && item[j].Equip == false)
                            {
                                //같은 아이템 2개를 합쳐 다음 아이템으로 생성
                                ItemList[index + 1].Count++;
                                ItemList[index].Count -= 2;

                                Console.WriteLine($"{ItemList[index].ItemName} 을(를) 획득했습니다.");
                            }
                            //예외 처리
                            else if (item[j].Count < 2)
                            {
                                Console.WriteLine("강화할 아이템이 부족합니다.");
                            }
                            else if (item[j].Equip == true)
                            {
                                Console.WriteLine("장착 중인 아이템입니다.");
                            }
                            break;
                        }
                }
            }
        }

        static void Inn()
        {
            while (true)
            {
                int[] choices = { 0, 1 };

                Console.WriteLine("여관");
                Console.WriteLine("휴식을 할 수 있는 여관입니다.");
                Console.WriteLine();
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        player.PlayerHP = player.PlayerMaxHP;
                        Console.WriteLine("휴식을 취해 체력을 회복했습니다.");
                        break;
                }
            }
        }

        static void Dungeon()
        {
            while (true)
            {
                //0부터 100까지의 난이도
                int[] choices = Enumerable.Range(0, 101).ToArray();

                Console.WriteLine("던전");
                Console.WriteLine("던전을 클리어해 돈과 경험치를 얻을 수 있습니다.");
                Console.WriteLine("수준에 맞는 던전을 선택해 주세요.");
                Console.WriteLine();
                //유저 편의성 스탯 창
                Console.WriteLine($"Lv. {player.PlayerLevel}");
                Console.WriteLine($"생명력 : {player.PlayerHP} / {player.PlayerMaxHP}");
                Console.WriteLine($"공격력 : {player.PlayerAtt} + {player.EquippedWeapon?.ItemAtt ?? 0}");
                Console.WriteLine($"방어력 : {player.PlayerDfn} + {player.EquippedArmor?.ItemDfn ?? 0}");
                Console.WriteLine($"Gold :  {player.PlayerGold}");
                Console.WriteLine();
                Console.WriteLine("[1~100]");
                Console.WriteLine("0. 나가기");
                Console.WriteLine($"마지막 던전 : {LastDungeon}층");

                int choice = Input(choices);

                switch (choice)
                {
                    case 0:
                        return;
                    default:
                        LastDungeon = choice;
                        DungeonFight(choice);
                        break;
                }
            }
        }

        static void DungeonFight(int difficulty)
        {
            //레벨링 함수들
            Random random = new Random();
            int dungeonAtt = 10 * difficulty;
            int dungeonDfn = 5 * difficulty;
            int gold = 1000 * difficulty;
            int att = player.PlayerAtt + (player.EquippedWeapon?.ItemAtt ?? 0);
            int dfn = player.PlayerDfn + (player.EquippedArmor?.ItemDfn ?? 0);

            float goldRate = (att - dungeonAtt) / 100f;
            int totalGold = random.Next(
                (int)(gold + (gold * goldRate)),
                (int)(1.5f * (gold + (gold * goldRate)))
            );

            int damage = random.Next(20, 35);
            int damageRate = dfn - dungeonDfn;
            int totalDamage = damage - damageRate;

            Console.WriteLine($"[ {difficulty}층 던전 입장 ]");
            Console.WriteLine();

            if (att < dungeonAtt)
            {
                totalGold /= 10;
                totalDamage *= 2;

                Console.WriteLine($"{player.PlayerName}은(는) 던전 공략에 실패했습니다.");
                Console.WriteLine($"{totalGold} G를 얻었습니다.");
                Console.WriteLine($"{totalDamage}의 피해를 입었습니다.");

                player.PlayerGold += totalGold;
                player.GetDamage(totalDamage);
            }
            else
            {
                Console.WriteLine($"{player.PlayerName}은(는) 던전을 공략했습니다!");
                Console.WriteLine($"{totalGold} G를 얻었습니다.");
                Console.WriteLine($"{totalDamage}의 피해를 입었습니다.");

                player.PlayerGold += totalGold;
                player.GetDamage(totalDamage);
            }

            Console.WriteLine();
        }

        //세이브 시스템
        static void SaveGame()
        {
            //필요 데이터를 선택
            var saveData = new SaveData
            {
                Player = player,
                Item = ItemList,
                LastDungeon = LastDungeon
            };

            //json형식으로 저장
            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("save.json", json);

            Console.WriteLine("게임이 저장되었습니다.");
            Console.WriteLine();
        }

        //로드 시스템
        static bool LoadGame()
        {
            if (!File.Exists("save.json"))
            {
                return false;
            }

            string json = File.ReadAllText("save.json");
            SaveData? saveData = JsonSerializer.Deserialize<SaveData>(json);

            if (saveData != null && saveData.Player != null && saveData.Item != null)
            {
                player = saveData.Player;

                // 타입 복원 로직
                ItemList = saveData.Item.Select(item =>
                {
                    if (item.Type == "Weapon")
                    {
                        return new Weapon
                        {
                            ItemName = item.ItemName,
                            ItemTooltip = item.ItemTooltip,
                            ItemAtt = item.ItemAtt,
                            Count = item.Count,
                            Tier = item.Tier,
                            Equip = item.Equip
                        };
                    }
                    else if (item.Type == "Armor")
                    {
                        return new Armor
                        {
                            ItemName = item.ItemName,
                            ItemTooltip = item.ItemTooltip,
                            ItemDfn = item.ItemDfn,
                            Count = item.Count,
                            Tier = item.Tier,
                            Equip = item.Equip
                        };
                    }
                    return item;
                }).ToList();

                // 장비 복원
                player.EquippedWeapon = ItemList.OfType<Weapon>().FirstOrDefault(x => x.Equip);
                player.EquippedArmor = ItemList.OfType<Armor>().FirstOrDefault(x => x.Equip);

                LastDungeon = saveData.LastDungeon;

                Console.WriteLine("게임을 불러왔습니다.");
                Console.WriteLine();
                return true;
            }
            else
            {
                return false;
            }
        }


        //인풋 확인 시스템
        static int Input(int[] choices)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("행동을 입력해 주세요.");
                Console.WriteLine();
                Console.Write(">>");
                string? choice = Console.ReadLine();

                //선택지마다 있는 배열을 참조
                //배열외의 입력이면 재입력 요청
                if (int.TryParse(choice, out int number) && choices.Contains(number))
                {
                    Console.Clear();
                    return number;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("다시 입력해 주세요.");
                }
            }
        }
    }
}
