using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atividade
{
    public partial class frmAtividade : Form
    {
        public frmAtividade()
        {
            InitializeComponent();
        }



        private void btnRun_Click(object sender, EventArgs e)
        {
            if (txtArquivo.Text.Trim().Equals(""))
            {
                MessageBox.Show(this, "Caminho do arquivo deve ser informado");
                txtArquivo.Focus();
                return;
            }

            if (!File.Exists(txtArquivo.Text.Trim()))
            {
                MessageBox.Show(this, "Arquivo inexistente!");
                txtArquivo.Focus();
                return;
            }

            Thread thread = new Thread(() => ExecutaAtividade(txtArquivo.Text.Trim()));
            thread.Name = "Atividade - Run";
            thread.Start();
        }


        private void ExecutaAtividade(string filePath)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                txtArquivo.Enabled = false;
                btnRun.Enabled = false;
            }));

            try
            {
                CodigoAtividade(filePath);

                this.Invoke(new MethodInvoker(delegate()
                {
                    MessageBox.Show(this, "Finalizado!");
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    MessageBox.Show(this, ex.Message);
                }));
            }
            finally
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    txtArquivo.Enabled = true;
                    btnRun.Enabled = true;
                }));
            }
        }




        private void CodigoAtividade(string filePath)
        {
            string[] linhasTxt = File.ReadAllLines(filePath);
            string[] primeiraLinha = linhasTxt[0].Split(' ');

            int linhas = Convert.ToInt32(primeiraLinha[0]);
            int colunas = Convert.ToInt32(primeiraLinha[1]);

            string[,] matriz = new string[linhas, colunas];
            int lAtual = -1;
            int cAtual = -1;
            int lSaida = -1;
            int cSaida = -1;

            for (int l = 1; l < linhasTxt.Length; l++)
            {
                string[] line = linhasTxt[l].Split(' ');

                for(int c = 0; c < line.Length; c++)
                {
                    string ll = line[c];
                    matriz[l - 1, c] = ll;

                    if(ll.Equals("X"))
                    {
                        lAtual = l - 1;
                        cAtual = c;
                    }
                    else if(ll.Equals("0") && (l == 1 || c == 0 || l == linhasTxt.Length -1 || c == line.Length -1))
                    {
                        lSaida = l - 1;
                        cSaida = c;
                    }
                }
            }

            List<string> resultado = new List<string>();
            
            resultado = procurarSaida(matriz, linhas, colunas, lAtual, cAtual, lSaida, cSaida);

            string folderPath = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            File.WriteAllLines(Path.Combine(folderPath, "saida" + fileName + ".txt"), resultado.ToArray(), Encoding.UTF8);
        }

        private static List<string> procurarSaida(string[,] matriz, int linhas, int colunas, int lAtual, int cAtual, int lSaida, int cSaida)
        {
            int extremidadeLinha = linhas - 1;
            int extremindadeColuna = colunas - 1;

            List<int> cAnteriores = new List<int>(); // armazena as colunas pecorridas
            List<int> lAnteriores = new List<int>();  // armazena as linhas percorridas
            List<string> direcoes = new List<string>(); // armazena as direcoes percorridas
            cAnteriores.Add(cAtual);
            lAnteriores.Add(lAtual);

            List<string> resultado = new List<string>();
            string direcao = "O";
            resultado.Add(direcao + " [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");
            direcoes.Add(direcao);

            int contadorVoltar = 0; // contador para não se perder ao regredir o caminho
            bool voltou = false; 
            bool achouSaida = lAtual == lSaida && cAtual == cSaida;
            while (!achouSaida)
            {
                // 1º Prioridade Cima
                if (lAtual - 1 >= 0 && matriz[lAtual - 1, cAtual].Equals("0"))
                {
                    direcao = "C";
                    contadorVoltar = 0; // reseta o contador
                    matriz[lAtual, cAtual] = "x"; // salva a direção já visitada
                    matriz[lAtual - 1, cAtual] = "X"; // salva a nova direção
                    lAtual = lAtual - 1; 
                    resultado.Add(direcao +" [" + (lAtual + 1) + ", " + (cAtual + 1) + "]"); 
                }
                // 2º Prioridade, Esquerda
                else if (cAtual - 1 >= 0 && matriz[lAtual, cAtual - 1].Equals("0"))
                {
                    direcao = "E";
                    contadorVoltar = 0;
                    matriz[lAtual, cAtual] = "x"; 
                    matriz[lAtual, cAtual - 1] = "X"; 
                    cAtual = cAtual - 1; 
                    resultado.Add(direcao + " [" + (lAtual + 1) + ", " + (cAtual + 1) + "]"); 
                }
                // 3º Prioridade, Direita
                else if (cAtual + 1 >= 0 && matriz[lAtual, cAtual + 1].Equals("0"))
                {
                    direcao = "D";
                    contadorVoltar = 0;
                    matriz[lAtual, cAtual] = "x"; 
                    matriz[lAtual, cAtual + 1] = "X"; 
                    cAtual = cAtual + 1; 
                    resultado.Add(direcao + " [" + (lAtual + 1) + ", " + (cAtual + 1) + "]"); 
                }
                // 4º Prioridade, Baixo
                else if (lAtual + 1 >= 0 && matriz[lAtual + 1, cAtual].Equals("0"))
                {
                    direcao = "B";
                    contadorVoltar = 0;
                    matriz[lAtual, cAtual] = "x"; 
                    matriz[lAtual + 1, cAtual] = "X"; 
                    lAtual = lAtual + 1; 
                    resultado.Add(direcao + " [" + (lAtual + 1) + ", " + (cAtual + 1) + "]"); 
                }

                // Caso se alcance um ponto em que não é possível se movimentar e/ou não tenham mais posições 
                // retorna usando o mesmo caminho utilizado até este ponto 
                // “sem-saída” até o último ponto onde teve mais de uma posição possível de movimento.
                else
                {
                    contadorVoltar += 1;
                    matriz[lAtual, cAtual] = "x";
                    
                    lAtual = lAnteriores[lAnteriores.Count - (contadorVoltar + 1)];
                    cAtual = cAnteriores[cAnteriores.Count - (contadorVoltar + 1)];

                    matriz[lAtual, cAtual] = "X";
                    
                    if (direcoes[direcoes.Count - (contadorVoltar)] == "C") { direcao = "B"; }
                    else if (direcoes[direcoes.Count - (contadorVoltar )] == "B") { direcao = "C"; }
                    else if (direcoes[direcoes.Count - (contadorVoltar )] == "D") { direcao = "E"; }
                    else if (direcoes[direcoes.Count - (contadorVoltar )] == "E") { direcao = "D"; }
                    
                    voltou = true;
                    resultado.Add(direcao + " [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");
                }
                if(!voltou){
                    cAnteriores.Add(cAtual);
                    lAnteriores.Add(lAtual);   
                    direcoes.Add(direcao);                 
                }
                voltou = false;
                achouSaida = lAtual == lSaida && cAtual == cSaida;
            }
            return resultado;
        }

    }
}
