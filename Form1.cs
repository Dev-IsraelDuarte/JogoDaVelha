using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JogoDaVelha
{
    public partial class Form1 : Form
    {
        Game game = new Game();
        Bot bot = new Bot();

        GameMode mode = GameMode.PvP;
        Difficulty difficulty = Difficulty.Easy;

        Button[] buttons;
        bool jogoIniciado = false;

        Label lblTitulo;
        Label lblStatus;

        ComboBox cbModo;
        ComboBox cbDificuldade;

        Button btnReset;
        Button btnSair;
        Button btnIniciar;

        Panel panelBotoes;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int left, int top, int right, int bottom,
            int width, int height
        );

        public Form1()
        {
            InitializeComponent();

            CriarInterfaceBase();
            CriarTabuleiro();

            Setup();
            ApplyModernDesign();
        }

        // ================= UI =================
        private void CriarInterfaceBase()
        {
            this.Text = "Jogo da Velha";
            this.Size = new Size(500, 650);
            this.BackColor = Color.FromArgb(18, 18, 18);

            lblTitulo = new Label()
            {
                Text = "JOGO DA VELHA",
                Location = new Point(130, 20),
                AutoSize = true
            };

            // 🔥 STATUS CENTRAL FIXO
            lblStatus = new Label()
            {
                Size = new Size(300, 30),
                Location = new Point((this.ClientSize.Width - 300) / 2, 480),
                TextAlign = ContentAlignment.MiddleCenter
            };

            cbModo = new ComboBox()
            {
                Location = new Point(50, 80),
                Size = new Size(180, 25)
            };
            cbModo.SelectedIndexChanged += cbModo_SelectedIndexChanged;

            cbDificuldade = new ComboBox()
            {
                Location = new Point(260, 80),
                Size = new Size(150, 25)
            };
            cbDificuldade.SelectedIndexChanged += cbDificuldade_SelectedIndexChanged;

            // 🔥 PANEL CENTRAL
            panelBotoes = new Panel();
            panelBotoes.Size = new Size(260, 50);
            panelBotoes.Location = new Point(
                (this.ClientSize.Width - panelBotoes.Width) / 2,
                520
            );

            btnReset = new Button()
            {
                Text = "Reiniciar",
                Size = new Size(110, 35),
                Location = new Point(0, 5)
            };
            btnReset.Click += btnReset_Click;

            btnSair = new Button()
            {
                Text = "Sair",
                Size = new Size(110, 35),
                Location = new Point(140, 5)
            };
            btnSair.Click += btnSair_Click;

            panelBotoes.Controls.Add(btnReset);
            panelBotoes.Controls.Add(btnSair);

            btnIniciar = new Button()
            {
                Text = "Iniciar Partida",
                Size = new Size(150, 35),
                Location = new Point((this.ClientSize.Width - 150) / 2, 580),
                Visible = false
            };
            btnIniciar.Click += btnIniciar_Click;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cbModo);
            this.Controls.Add(cbDificuldade);
            this.Controls.Add(panelBotoes);
            this.Controls.Add(btnIniciar);
        }

        private void CriarTabuleiro()
        {
            int tamanho = 100;
            int espacamento = 10;
            int grid = (tamanho * 3) + (espacamento * 2);

            int startX = (this.ClientSize.Width - grid) / 2;
            int startY = 140;

            buttons = new Button[9];
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button();

                    btn.Name = "btn" + count;
                    btn.Size = new Size(tamanho, tamanho);
                    btn.Location = new Point(
                        startX + (j * (tamanho + espacamento)),
                        startY + (i * (tamanho + espacamento))
                    );

                    btn.Tag = count;
                    btn.Click += Button_Click;

                    this.Controls.Add(btn);
                    buttons[count++] = btn;
                }
            }
        }

        private void Setup()
        {
            cbModo.Items.AddRange(new string[]
            {
                "Player vs Player",
                "Player vs Bot",
                "Bot vs Bot"
            });

            cbDificuldade.Items.AddRange(new string[]
            {
                "Fácil", "Médio", "Difícil", "Impossível"
            });

            cbModo.SelectedIndex = 0;
            cbDificuldade.SelectedIndex = 0;

            ResetGame();
        }

        private void ApplyModernDesign()
        {
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;

            lblStatus.Font = new Font("Segoe UI", 12);
            lblStatus.ForeColor = Color.LightGray;

            StyleButton(btnReset, Color.FromArgb(70, 130, 180));
            StyleButton(btnSair, Color.FromArgb(180, 50, 50));
            StyleButton(btnIniciar, Color.FromArgb(60, 180, 100));

            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(40, 40, 40);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 28, FontStyle.Bold);

                btn.Region = Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 25, 25)
                );

                btn.MouseEnter += (s, e) =>
                    btn.BackColor = Color.FromArgb(70, 70, 70);

                btn.MouseLeave += (s, e) =>
                    btn.BackColor = Color.FromArgb(40, 40, 40);

                // 🔥 ANIMAÇÃO CLICK
                btn.MouseDown += async (s, e) =>
                {
                    btn.Size = new Size(95, 95);
                    btn.Location = new Point(btn.Location.X + 2, btn.Location.Y + 2);

                    await Task.Delay(80);

                    btn.Size = new Size(100, 100);
                    btn.Location = new Point(btn.Location.X - 2, btn.Location.Y - 2);
                };
            }
        }

        private void StyleButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            btn.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 20, 20)
            );
        }

        private void DestacarVitoria(int[] posicoes)
        {
            foreach (int i in posicoes)
                buttons[i].BackColor = Color.FromArgb(60, 180, 100);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (!jogoIniciado && mode != GameMode.PvP)
            {
                MessageBox.Show("Clique em 'Iniciar Partida'!");
                return;
            }

            Button btn = sender as Button;
            int index = Convert.ToInt32(btn.Tag);

            if (!game.MakeMove(index)) return;

            btn.Text = game.Board[index];

            if (CheckEnd()) return;

            HandleBot();
        }

        private async void HandleBot()
        {
            if (!jogoIniciado) return;

            // Player vs Bot → só o O joga
            if (mode == GameMode.PvBot && game.CurrentPlayer == "O")
            {
                await Task.Delay(400);
                MakeBotMove();
            }

            // Bot vs Bot → ambos jogam automaticamente
            if (mode == GameMode.BotVsBot)
            {
                await Task.Delay(400);
                MakeBotMove();
            }
        }

        private void MakeBotMove()
        {
            int move = bot.GetMove(game.Board, difficulty, "O", "X");

            game.MakeMove(move);
            buttons[move].Text = game.Board[move];

            if (CheckEnd()) return;

            HandleBot();
        }

        private bool CheckEnd()
        {
            var result = game.CheckWinner();

            if (result != null)
            {
                if (result == "X" || result == "O")
                {
                    int[][] wins =
                    {
                        new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8},
                        new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8},
                        new[] {0,4,8}, new[] {2,4,6}
                    };

                    foreach (var w in wins)
                    {
                        if (game.Board[w[0]] == result &&
                            game.Board[w[1]] == result &&
                            game.Board[w[2]] == result)
                        {
                            DestacarVitoria(w);
                            break;
                        }
                    }
                }

                MessageBox.Show($"Resultado: {result}");
                ResetGame();
                return true;
            }

            lblStatus.Text = $"Vez: {game.CurrentPlayer}";
            return false;
        }

        private void ResetGame()
        {
            game.Reset();

            foreach (var btn in buttons)
            {
                btn.Text = "";
                btn.BackColor = Color.FromArgb(40, 40, 40);
            }

            SortearInicio();
            jogoIniciado = mode == GameMode.PvP;
        }

        private void SortearInicio()
        {
            game.CurrentPlayer = new Random().Next(2) == 0 ? "X" : "O";
            lblStatus.Text = $"Começa: {game.CurrentPlayer}";
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            jogoIniciado = true;
            HandleBot();
        }

        private void cbModo_SelectedIndexChanged(object sender, EventArgs e)
        {
            mode = (GameMode)cbModo.SelectedIndex;

            bool vsBot = mode != GameMode.PvP;

            cbDificuldade.Enabled = mode == GameMode.PvBot;
            btnIniciar.Visible = vsBot;

            ResetGame();
        }

        private void cbDificuldade_SelectedIndexChanged(object sender, EventArgs e)
        {
            difficulty = (Difficulty)cbDificuldade.SelectedIndex;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}