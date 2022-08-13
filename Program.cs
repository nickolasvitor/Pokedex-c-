//o dotnet 6 tem uma nova feature, caso não declare explicitamente que uma variavel possa ser nula, ele mostrar um -warning- pedindo pra explicitar essa nulidade, tem alguns warning no programa, mas é por conta disso.
using Newtonsoft.Json;
using Pokemons;
using Types;
namespace ProjectPokedex
{
    class Program
    {
        static async Task Main()
        {   int x=0;
            while(x==0)
            {
                System.Console.WriteLine("Digite o número do pokémon que deseja consultar:");
                string poke = Console.ReadLine();
                int pokeInt = int.Parse(poke);
                if(pokeInt>=1 && pokeInt<=905)
                {
                HttpClient client = new HttpClient{BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/")};
                var response = await client.GetAsync(poke);
                var stringResponse = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<Pokemon>(stringResponse);
                
                //Primeira fase
                
                Console.WriteLine($"nome do pokémon: {pokemon?.Name}\n");
                string absoluteUri = pokemon.Sprites.FrontDefault.AbsoluteUri;
                Console.WriteLine($"Foto frontal do pokémon {pokemon.Name}:\n{absoluteUri}\n");


                //Segunda fase
                List<string> allMoves = new List<string>();
                foreach(var item in pokemon.Moves)
                    {
                    allMoves.Add(item.MoveMove.Name.ToString());
                    }
                System.Console.Write($"Lista de golpes do pokemon {pokemon.Name}: ");
                System.Console.WriteLine((JsonConvert.SerializeObject(allMoves)));
                System.Console.Write("\n");
                
                
                Dictionary<string, string> dStats = new Dictionary<string, string>();
                int i=0;
                foreach (var item in pokemon.Stats)
                    {
                    i=i+1;
                    dStats.Add($"base_stat_{i}",item.BaseStat.ToString());
                    dStats.Add($"name_{i}",item.StatStat.Name);
                    }
                System.Console.Write($"Dicionário de stats do pokemon {pokemon.Name}: ");
                Console.WriteLine((JsonConvert.SerializeObject(dStats)));
                System.Console.Write("\n");

                //Terceira fase
                List<string> doubleDamageFrom = new List<string>();
                List<string> doubleDamageTo = new List<string>();
                List<string> noDamageFrom = new List<string>();
                List<string> noDamageTo = new List<string>();

                
                foreach (var item in pokemon.Types)
                    {   
                        HttpClient client1 = new HttpClient();
                        var response1 = await client1.GetAsync($"{item.Type.Url}");
                        var stringResponse1 = await response1.Content.ReadAsStringAsync();
                        var type = JsonConvert.DeserializeObject<PokeType>(stringResponse1);

                        
                        foreach(var obj in type.DamageRelations.DoubleDamageFrom)
                        {
                        doubleDamageFrom.Add($"Tipo do seu pokémon {item.Type.Name}: Leva double damage para {obj.Name}");
                        }
                        foreach(var obj in type.DamageRelations.DoubleDamageTo)
                        {
                        doubleDamageTo.Add($"Tipo do seu pokémon {item.Type.Name}: Dá double damage em {obj.Name}");
                        }
                        if(type.DamageRelations.NoDamageFrom.Count == 0)
                        {
                        noDamageFrom.Add($"Tipo do seu pokémon {item.Type.Name}: Todos os pokémons podem causar certo tipo de dano a esse tipo");
                        }else{
                            noDamageFrom.Add($"Tipo do seu pokémon {item.Type.Name}: Não leva dano de {type.DamageRelations.NoDamageFrom}");
                        }

                        if(type.DamageRelations.NoDamageTo.Count == 0)
                        {
                            noDamageTo.Add($"Tipo do seu pokémon {item.Type.Name}: Todos os pokémons podem causar certo tipo de dano a esse tipo");
                        }else{
                            noDamageTo.Add($"Tipo do seu pokémon {item.Type.Name}: Não dá dano no tipo {type.DamageRelations.NoDamageTo[0].Name}");
                        }
                        
                    }
                    System.Console.Write("Leva dano duplo de: ");
                    System.Console.WriteLine(JsonConvert.SerializeObject(doubleDamageFrom));System.Console.Write("\n");
                    System.Console.Write("Realiza dano duplo em: ");
                    System.Console.WriteLine(JsonConvert.SerializeObject(doubleDamageTo));System.Console.Write("\n");
                    System.Console.Write("Não toma dano de: ");
                    System.Console.WriteLine(JsonConvert.SerializeObject(noDamageFrom));System.Console.Write("\n");
                    System.Console.Write("Não gera dano em ");
                    System.Console.WriteLine(JsonConvert.SerializeObject(noDamageTo));System.Console.Write("\n");

                    //Saindo do laço e finalizando o programa
                    System.Console.WriteLine("Deseja fazer uma nova consulta? S/N?");System.Console.Write("\n");

                    string finish = Console.ReadLine();
                    if(String.Equals(finish.ToUpper(),"N"))break;
        
                

              
                }else{System.Console.WriteLine("Por favor informar entre 1 a 905");}
                

            }      
        }
    }
}