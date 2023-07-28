namespace GMapAddingMarkersOnMouseClick
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.btnDropPin = new System.Windows.Forms.Button();
            this.btnDrawLine = new System.Windows.Forms.Button();
            this.btnDrawPolygon = new System.Windows.Forms.Button();
            this.btnClearMarkers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gmap
            // 
            this.gmap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemory = 5;
            this.gmap.Location = new System.Drawing.Point(12, 12);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 2;
            this.gmap.MinZoom = 2;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(992, 744);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 0D;
            // 
            // btnDropPin
            // 
            this.btnDropPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDropPin.BackColor = System.Drawing.Color.Black;
            this.btnDropPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDropPin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btnDropPin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDropPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDropPin.ForeColor = System.Drawing.Color.White;
            this.btnDropPin.Location = new System.Drawing.Point(1027, 29);
            this.btnDropPin.Name = "btnDropPin";
            this.btnDropPin.Size = new System.Drawing.Size(121, 55);
            this.btnDropPin.TabIndex = 1;
            this.btnDropPin.Text = "Drop Pin";
            this.btnDropPin.UseVisualStyleBackColor = false;
            this.btnDropPin.Click += new System.EventHandler(this.btnDropPin_Click);
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDrawLine.BackColor = System.Drawing.Color.Black;
            this.btnDrawLine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDrawLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btnDrawLine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDrawLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawLine.ForeColor = System.Drawing.Color.White;
            this.btnDrawLine.Location = new System.Drawing.Point(1027, 146);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(121, 55);
            this.btnDrawLine.TabIndex = 2;
            this.btnDrawLine.Text = "Draw Line";
            this.btnDrawLine.UseVisualStyleBackColor = false;
            this.btnDrawLine.Click += new System.EventHandler(this.btnDrawLine_Click);
            // 
            // btnDrawPolygon
            // 
            this.btnDrawPolygon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDrawPolygon.BackColor = System.Drawing.Color.Black;
            this.btnDrawPolygon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDrawPolygon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btnDrawPolygon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDrawPolygon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawPolygon.ForeColor = System.Drawing.Color.White;
            this.btnDrawPolygon.Location = new System.Drawing.Point(1027, 270);
            this.btnDrawPolygon.Name = "btnDrawPolygon";
            this.btnDrawPolygon.Size = new System.Drawing.Size(121, 57);
            this.btnDrawPolygon.TabIndex = 3;
            this.btnDrawPolygon.Text = "Draw Polygon";
            this.btnDrawPolygon.UseVisualStyleBackColor = false;
            this.btnDrawPolygon.Click += new System.EventHandler(this.btnDrawPolygon_Click);
            // 
            // btnClearMarkers
            // 
            this.btnClearMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearMarkers.BackColor = System.Drawing.Color.Black;
            this.btnClearMarkers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearMarkers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btnClearMarkers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClearMarkers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearMarkers.ForeColor = System.Drawing.Color.White;
            this.btnClearMarkers.Location = new System.Drawing.Point(1027, 394);
            this.btnClearMarkers.Name = "btnClearMarkers";
            this.btnClearMarkers.Size = new System.Drawing.Size(121, 57);
            this.btnClearMarkers.TabIndex = 4;
            this.btnClearMarkers.Text = "Clear Markers";
            this.btnClearMarkers.UseVisualStyleBackColor = false;
            this.btnClearMarkers.Click += new System.EventHandler(this.btnClearMarkers_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 768);
            this.Controls.Add(this.btnClearMarkers);
            this.Controls.Add(this.btnDrawPolygon);
            this.Controls.Add(this.btnDrawLine);
            this.Controls.Add(this.btnDropPin);
            this.Controls.Add(this.gmap);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.Button btnDropPin;
        private System.Windows.Forms.Button btnDrawLine;
        private System.Windows.Forms.Button btnDrawPolygon;
        private System.Windows.Forms.Button btnClearMarkers;
    }
}

