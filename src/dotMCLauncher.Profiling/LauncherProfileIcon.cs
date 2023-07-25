namespace dotMCLauncher.Profiling
{
    public static class LauncherProfileIcon
    {
        public enum Icon
        {
            BEDROCK,
            BOOKSHELF,
            BRICK,
            CAKE,
            CARVED_PUMPKIN,
            CHEST,
            CLAY,
            COAL_BLOCK,
            COAL_ORE,
            COBBLESTONE,
            CRAFTING_TABLE,
            CREEPER_HEAD,
            DIAMOND_BLOCK,
            DIAMOND_ORE,
            DIRT,
            DIRT_PODZOL,
            DIRT_SNOW,
            EMERALD_BLOCK,
            EMERALD_ORE,
            ENCHANTING_TABLE,
            END_STONE,
            FARMLAND,
            FURNACE, // null entry
            FURNACE_ON,
            GLASS,
            GLAZED_TERRACOTTA_LIGHT_BLUE,
            GLAZED_TERRACOTTA_ORANGE,
            GLAZED_TERRACOTTA_WHITE,
            GLOWSTONE,
            GOLD_BLOCK,
            GOLD_ORE,
            GRAVEL,
            HARDENED_CLAY,
            ICE_PACKED,
            IRON_BLOCK,
            IRON_ORE,
            LAPIS_ORE,
            LEAVES_BIRCH,
            LEAVES_JUNGLE,
            LEAVES_OAK,
            LEAVES_SPRUCE,
            LECTERN_BOOK,
            LOG_ACACIA,
            LOG_BIRCH,
            LOG_DARK_OAK,
            LOG_JUNGLE,
            LOG_OAK,
            LOG_SPRUCE,
            MYCELIUM,
            NETHER_BRICK,
            NETHERRACK,
            OBSIDIAN,
            PLANKS_ACACIA,
            PLANKS_BIRCH,
            PLANKS_DARK_OAK,
            PLANKS_JUNGLE,
            PLANKS_OAK,
            PLANKS_SPRUCE,
            QUARTZ_ORE,
            RED_SAND,
            RED_SANDSTONE,
            REDSTONE_BLOCK,
            REDSTONE_ORE,
            SAND,
            SANDSTONE,
            SKELETON_SKULL,
            SNOW,
            SOUL_SAND,
            STONE,
            STONE_ANDESITE,
            STONE_DIORITE,
            STONE_GRANITE,
            TNT,
            WATER,
            WOOL
        }

        public static Icon GetIcon(string iconEntry)
        {
            switch (iconEntry) {
                case "Bedrock":
                    return Icon.BEDROCK;
                case "Bookshelf":
                    return Icon.BOOKSHELF;
                case "Brick":
                    return Icon.BRICK;
                case "Cake":
                    return Icon.CAKE;
                case "Carved_Pumpkin":
                    return Icon.CARVED_PUMPKIN;
                case "Chest":
                    return Icon.CHEST;
                case "Clay":
                    return Icon.CLAY;
                case "Coal_Block":
                    return Icon.COAL_BLOCK;
                case "Coal_Ore":
                    return Icon.COAL_ORE;
                case "Cobblestone":
                    return Icon.COBBLESTONE;
                case "Crafting_Table":
                    return Icon.CRAFTING_TABLE;
                case "Creeper_Head":
                    return Icon.CREEPER_HEAD;
                case "Diamond_Block":
                    return Icon.DIAMOND_BLOCK;
                case "Diamond_Ore":
                    return Icon.DIAMOND_ORE;
                case "Dirt":
                    return Icon.DIRT;
                case "Dirt_Podzol":
                    return Icon.DIRT_PODZOL;
                case "Dirt_Snow":
                    return Icon.DIRT_SNOW;
                case "Emerald_Block":
                    return Icon.EMERALD_BLOCK;
                case "Emerald_Ore":
                    return Icon.EMERALD_ORE;
                case "Enchanting_Table":
                    return Icon.ENCHANTING_TABLE;
                case "End_Stone":
                    return Icon.END_STONE;
                case "Farmland":
                    return Icon.FARMLAND;
                case Icon.FURNACE:
                    return "Furnace";
                case "Furnace_On":
                    return Icon.FURNACE_ON;
                case "Glass":
                    return Icon.GLASS;
                case "Glazed_Terracotta_Light_Blue":
                    return Icon.GLAZED_TERRACOTTA_LIGHT_BLUE;
                case "Glazed_Terracotta_Orange":
                    return Icon.GLAZED_TERRACOTTA_ORANGE;
                case "Glazed_Terracotta_White":
                    return Icon.GLAZED_TERRACOTTA_WHITE;
                case "Glowstone":
                    return Icon.GLOWSTONE;
                case "Gold_Block":
                    return Icon.GOLD_BLOCK;
                case "Gold_Ore":
                    return Icon.GOLD_ORE;
                case "Gravel":
                    return Icon.GRAVEL;
                case "Hardened_Clay":
                    return Icon.HARDENED_CLAY;
                case "Ice_Packed":
                    return Icon.ICE_PACKED;
                case "Iron_Block":
                    return Icon.IRON_BLOCK;
                case "Iron_Ore":
                    return Icon.IRON_ORE;
                case "Lapis_Ore":
                    return Icon.LAPIS_ORE;
                case "Leaves_Birch":
                    return Icon.LEAVES_BIRCH;
                case "Leaves_Jungle":
                    return Icon.LEAVES_JUNGLE;
                case "Leaves_Oak":
                    return Icon.LEAVES_OAK;
                case "Leaves_Spruce":
                    return Icon.LEAVES_SPRUCE;
                case "Lectern_Book":
                    return Icon.LECTERN_BOOK;
                case "Log_Acacia":
                    return Icon.LOG_ACACIA;
                case "Log_Birch":
                    return Icon.LOG_BIRCH;
                case "Log_DarkOak":
                    return Icon.LOG_DARK_OAK;
                case "Log_Jungle":
                    return Icon.LOG_JUNGLE;
                case "Log_Oak":
                    return Icon.LOG_OAK;
                case "Log_Spruce":
                    return Icon.LOG_SPRUCE;
                case "Mycelium":
                    return Icon.MYCELIUM;
                case "Nether_Brick":
                    return Icon.NETHER_BRICK;
                case "Netherrack":
                    return Icon.NETHERRACK;
                case "Obsidian":
                    return Icon.OBSIDIAN;
                case "Planks_Acacia":
                    return Icon.PLANKS_ACACIA;
                case "Planks_Birch":
                    return Icon.PLANKS_BIRCH;
                case "Planks_DarkOak":
                    return Icon.PLANKS_DARK_OAK;
                case "Planks_Jungle":
                    return Icon.PLANKS_JUNGLE;
                case "Planks_Oak":
                    return Icon.PLANKS_OAK;
                case "Planks_Spruce":
                    return Icon.PLANKS_SPRUCE;
                case "Quartz_Ore":
                    return Icon.QUARTZ_ORE;
                case "Red_Sand":
                    return Icon.RED_SAND;
                case "Red_Sandstone":
                    return Icon.RED_SANDSTONE;
                case "Redstone_Block":
                    return Icon.REDSTONE_BLOCK;
                case "Redstone_Ore":
                    return Icon.REDSTONE_ORE;
                case "Sand":
                    return Icon.SAND;
                case "Sandstone":
                    return Icon.SANDSTONE;
                case "Skeleton_Skull":
                    return Icon.SKELETON_SKULL;
                case "Snow":
                    return Icon.SNOW;
                case "Soul_Sand":
                    return Icon.SOUL_SAND;
                case "Stone":
                    return Icon.STONE;
                case "Stone_Andesite":
                    return Icon.STONE_ANDESITE;
                case "Stone_Diorite":
                    return Icon.STONE_DIORITE;
                case "Stone_Granite":
                    return Icon.STONE_GRANITE;
                case "TNT":
                    return Icon.TNT;
                case "Water":
                    return Icon.WATER;
                case "Wool":
                    return Icon.WOOL;
                default:
                    return Icon.FURNACE;
            }
        }

        public static string GetString(Icon icon)
        {
            switch (icon) {
                case Icon.BEDROCK:
                    return "Bedrock";
                case Icon.BOOKSHELF:
                    return "Bookshelf";
                case Icon.BRICK:
                    return "Brick";
                case Icon.CAKE:
                    return "Cake";
                case Icon.CARVED_PUMPKIN:
                    return "Carved_Pumpkin";
                case Icon.CHEST:
                    return "Chest";
                case Icon.CLAY:
                    return "Clay";
                case Icon.COAL_BLOCK:
                    return "Coal_Block";
                case Icon.COAL_ORE:
                    return "Coal_Ore";
                case Icon.COBBLESTONE:
                    return "Cobblestone";
                case Icon.CRAFTING_TABLE:
                    return "Crafting_Table";
                case Icon.CREEPER_HEAD:
                    return "Creeper_Head";
                case Icon.DIAMOND_BLOCK:
                    return "Diamond_Block";
                case Icon.DIAMOND_ORE:
                    return "Diamond_Ore";
                case Icon.DIRT:
                    return "Dirt";
                case Icon.DIRT_PODZOL:
                    return "Dirt_Podzol";
                case Icon.DIRT_SNOW:
                    return "Dirt_Snow";
                case Icon.EMERALD_BLOCK:
                    return "Emerald_Block";
                case Icon.EMERALD_ORE:
                    return "Emerald_Ore";
                case Icon.ENCHANTING_TABLE:
                    return "Enchanting_Table";
                case Icon.END_STONE:
                    return "End_Stone";
                case Icon.FARMLAND:
                    return "Farmland";
                case Icon.FURNACE:
                    return "Furnace";
                case Icon.FURNACE_ON:
                    return "Furnace_On";
                case Icon.GLASS:
                    return "Glass";
                case Icon.GLAZED_TERRACOTTA_LIGHT_BLUE:
                    return "Glazed_Terracotta_Light_Blue";
                case Icon.GLAZED_TERRACOTTA_ORANGE:
                    return "Glazed_Terracotta_Orange";
                case Icon.GLAZED_TERRACOTTA_WHITE:
                    return "Glazed_Terracotta_White";
                case Icon.GLOWSTONE:
                    return "Glowstone";
                case Icon.GOLD_BLOCK:
                    return "Gold_Block";
                case Icon.GOLD_ORE:
                    return "Gold_Ore";
                case Icon.GRAVEL:
                    return "Gravel";
                case Icon.HARDENED_CLAY:
                    return "Hardened_Clay";
                case Icon.ICE_PACKED:
                    return "Ice_Packed";
                case Icon.IRON_BLOCK:
                    return "Iron_Block";
                case Icon.IRON_ORE:
                    return "Iron_Ore";
                case Icon.LAPIS_ORE:
                    return "Lapis_Ore";
                case Icon.LEAVES_BIRCH:
                    return "Leaves_Birch";
                case Icon.LEAVES_JUNGLE:
                    return "Leaves_Jungle";
                case Icon.LEAVES_OAK:
                    return "Leaves_Oak";
                case Icon.LEAVES_SPRUCE:
                    return "Leaves_Spruce";
                case Icon.LECTERN_BOOK:
                    return "Lectern_Book";
                case Icon.LOG_ACACIA:
                    return "Log_Acacia";
                case Icon.LOG_BIRCH:
                    return "Log_Birch";
                case Icon.LOG_DARK_OAK:
                    return "Log_DarkOak";
                case Icon.LOG_JUNGLE:
                    return "Log_Jungle";
                case Icon.LOG_OAK:
                    return "Log_Oak";
                case Icon.LOG_SPRUCE:
                    return "Log_Spruce";
                case Icon.MYCELIUM:
                    return "Mycelium";
                case Icon.NETHER_BRICK:
                    return "Nether_Brick";
                case Icon.NETHERRACK:
                    return "Netherrack";
                case Icon.OBSIDIAN:
                    return "Obsidian";
                case Icon.PLANKS_ACACIA:
                    return "Planks_Acacia";
                case Icon.PLANKS_BIRCH:
                    return "Planks_Birch";
                case Icon.PLANKS_DARK_OAK:
                    return "Planks_DarkOak";
                case Icon.PLANKS_JUNGLE:
                    return "Planks_Jungle";
                case Icon.PLANKS_OAK:
                    return "Planks_Oak";
                case Icon.PLANKS_SPRUCE:
                    return "Planks_Spruce";
                case Icon.QUARTZ_ORE:
                    return "Quartz_Ore";
                case Icon.RED_SAND:
                    return "Red_Sand";
                case Icon.RED_SANDSTONE:
                    return "Red_Sandstone";
                case Icon.REDSTONE_BLOCK:
                    return "Redstone_Block";
                case Icon.REDSTONE_ORE:
                    return "Redstone_Ore";
                case Icon.SAND:
                    return "Sand";
                case Icon.SANDSTONE:
                    return "Sandstone";
                case Icon.SKELETON_SKULL:
                    return "Skeleton_Skull";
                case Icon.SNOW:
                    return "Snow";
                case Icon.SOUL_SAND:
                    return "Soul_Sand";
                case Icon.STONE:
                    return "Stone";
                case Icon.STONE_ANDESITE:
                    return "Stone_Andesite";
                case Icon.STONE_DIORITE:
                    return "Stone_Diorite";
                case Icon.STONE_GRANITE:
                    return "Stone_Granite";
                case Icon.TNT:
                    return "TNT";
                case Icon.WATER:
                    return "Water";
                case Icon.WOOL:
                    return "Wool";
                default:
                    return null;
            }
        }
    }
}
