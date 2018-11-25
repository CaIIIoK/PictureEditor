using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace PictureEditor
{
    public partial class Form1 : Form
    {
        private Image imageFile;
        private bool isOpened = false;

        public Form1()
        {
            InitializeComponent();

        }

        public void OpenImage()
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                imageFile = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = imageFile;
                isOpened = true;
            }
        }

        public void SaveImage()
        {
            if(isOpened)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Images|*.png,*.bmp,*.jpg";
                // Default format
                ImageFormat imageFormat = ImageFormat.Png;
                if(saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string extention = Path.GetExtension(saveFileDialog.FileName);
                    switch(extention)
                    {
                        case ".jpg":
                            imageFormat = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            imageFormat = ImageFormat.Bmp;
                            break;
                    }
                    pictureBox1.Image.Save(saveFileDialog.FileName, imageFormat);
                }
            }
            else
            {
                MessageBox.Show("Nothing to save!");
            }
        }       

        public void Reload()
        {
            if(!isOpened)
            {
                MessageBox.Show("Open the image than apply changes.");
            }
            else
            {
                if(isOpened)
                {
                    imageFile = Image.FromFile(openFileDialog1.FileName);
                    pictureBox1.Image = imageFile;
                    isOpened = true;
                }

            }
        }

        public void UseRGBTrackBars()
        {
            float changered = redBar.Value * 0.1f;
            float changegreen = greenBar.Value * 0.1f;
            float changeblue = blueBar.Value * 0.1f;

            Reload();
            if (isOpened)
            {
                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMatrix colorMatrixPicture = new ColorMatrix(new float[][]
                {
                    new float[]{1+changered, 0, 0, 0, 0},
                    new float[]{0, 1+changegreen, 0, 0, 0},
                    new float[]{0, 0, 1+changeblue, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                imageAttributes.SetColorMatrix(colorMatrixPicture);
                Graphics g = Graphics.FromImage(bmpInverted);
                g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                g.Dispose();
                pictureBox1.Image = bmpInverted;
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void redBar_ValueChanged(object sender, EventArgs e)
        {
            UseRGBTrackBars();
        }

        private void greenBar_ValueChanged(object sender, EventArgs e)
        {
            UseRGBTrackBars();
        }

        private void blueBar_ValueChanged(object sender, EventArgs e)
        {
            UseRGBTrackBars();
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            Reload();
            redBar.Value = 0;
            greenBar.Value = 0;
            blueBar.Value = 0;
        }
    }
}
