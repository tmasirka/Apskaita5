<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class F_JournalEntry
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim ContentLabel As System.Windows.Forms.Label
        Dim DateLabel As System.Windows.Forms.Label
        Dim DocNumberLabel As System.Windows.Forms.Label
        Dim DocTypeHumanReadableLabel As System.Windows.Forms.Label
        Dim IDLabel As System.Windows.Forms.Label
        Dim PersonLabel As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Dim Label2 As System.Windows.Forms.Label
        Dim Label3 As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_JournalEntry))
        Me.PersonAccGridComboBox = New AccControlsWinForms.AccListComboBox
        Me.JournalEntryBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ContentTextBox = New System.Windows.Forms.TextBox
        Me.CreditSumAccBox = New AccControlsWinForms.AccTextBox
        Me.DateDateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.DebetSumAccBox = New AccControlsWinForms.AccTextBox
        Me.DocNumberTextBox = New System.Windows.Forms.TextBox
        Me.DocTypeHumanReadableTextBox = New System.Windows.Forms.TextBox
        Me.IDTextBox = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ViewRelationsButton = New System.Windows.Forms.Button
        Me.NewItemButton = New System.Windows.Forms.Button
        Me.nCancelButton = New System.Windows.Forms.Button
        Me.ApplyButton = New System.Windows.Forms.Button
        Me.nOkButton = New System.Windows.Forms.Button
        Me.LoadTemplateButton = New System.Windows.Forms.Button
        Me.TemplateComboBox = New System.Windows.Forms.ComboBox
        Me.DebetListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.CreditListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel9 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.ViewDocumentButton = New System.Windows.Forms.Button
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel
        Me.DebitListDataListView = New BrightIdeasSoftware.DataListView
        Me.OlvColumn4 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn5 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn6 = New BrightIdeasSoftware.OLVColumn
        Me.CreditListDataListView = New BrightIdeasSoftware.DataListView
        Me.OlvColumn1 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn2 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn3 = New BrightIdeasSoftware.OLVColumn
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel
        Me.ProgressFiller1 = New AccControlsWinForms.ProgressFiller
        Me.ProgressFiller2 = New AccControlsWinForms.ProgressFiller
        Me.ErrorWarnInfoProvider1 = New AccControlsWinForms.ErrorWarnInfoProvider(Me.components)
        ContentLabel = New System.Windows.Forms.Label
        DateLabel = New System.Windows.Forms.Label
        DocNumberLabel = New System.Windows.Forms.Label
        DocTypeHumanReadableLabel = New System.Windows.Forms.Label
        IDLabel = New System.Windows.Forms.Label
        PersonLabel = New System.Windows.Forms.Label
        Label1 = New System.Windows.Forms.Label
        Label2 = New System.Windows.Forms.Label
        Label3 = New System.Windows.Forms.Label
        CType(Me.JournalEntryBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.DebetListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreditListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel9.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        CType(Me.DebitListDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreditListDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContentLabel
        '
        ContentLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ContentLabel.AutoSize = True
        ContentLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ContentLabel.Location = New System.Drawing.Point(37, 82)
        ContentLabel.Name = "ContentLabel"
        ContentLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        ContentLabel.Size = New System.Drawing.Size(52, 18)
        ContentLabel.TabIndex = 0
        ContentLabel.Text = "Turinys:"
        '
        'DateLabel
        '
        DateLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DateLabel.AutoSize = True
        DateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DateLabel.Location = New System.Drawing.Point(51, 27)
        DateLabel.Name = "DateLabel"
        DateLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        DateLabel.Size = New System.Drawing.Size(38, 18)
        DateLabel.TabIndex = 4
        DateLabel.Text = "Data:"
        '
        'DocNumberLabel
        '
        DocNumberLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DocNumberLabel.AutoSize = True
        DocNumberLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DocNumberLabel.Location = New System.Drawing.Point(267, 0)
        DocNumberLabel.Name = "DocNumberLabel"
        DocNumberLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        DocNumberLabel.Size = New System.Drawing.Size(85, 18)
        DocNumberLabel.TabIndex = 8
        DocNumberLabel.Text = "Dok. numeris:"
        '
        'DocTypeHumanReadableLabel
        '
        DocTypeHumanReadableLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DocTypeHumanReadableLabel.AutoSize = True
        DocTypeHumanReadableLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DocTypeHumanReadableLabel.Location = New System.Drawing.Point(191, 0)
        DocTypeHumanReadableLabel.Name = "DocTypeHumanReadableLabel"
        DocTypeHumanReadableLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        DocTypeHumanReadableLabel.Size = New System.Drawing.Size(106, 18)
        DocTypeHumanReadableLabel.TabIndex = 12
        DocTypeHumanReadableLabel.Text = "Dokumento tipas:"
        '
        'IDLabel
        '
        IDLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        IDLabel.AutoSize = True
        IDLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        IDLabel.Location = New System.Drawing.Point(65, 0)
        IDLabel.Name = "IDLabel"
        IDLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        IDLabel.Size = New System.Drawing.Size(24, 18)
        IDLabel.TabIndex = 14
        IDLabel.Text = "ID:"
        '
        'PersonLabel
        '
        PersonLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        PersonLabel.AutoSize = True
        PersonLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        PersonLabel.Location = New System.Drawing.Point(3, 54)
        PersonLabel.Name = "PersonLabel"
        PersonLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        PersonLabel.Size = New System.Drawing.Size(86, 18)
        PersonLabel.TabIndex = 18
        PersonLabel.Text = "Kontrahentas:"
        '
        'Label1
        '
        Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Label1.AutoSize = True
        Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label1.Location = New System.Drawing.Point(3, 0)
        Label1.Name = "Label1"
        Label1.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Label1.Size = New System.Drawing.Size(83, 21)
        Label1.TabIndex = 50
        Label1.Text = "DEBETAS:"
        '
        'Label2
        '
        Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Label2.AutoSize = True
        Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label2.Location = New System.Drawing.Point(3, 0)
        Label2.Name = "Label2"
        Label2.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Label2.Size = New System.Drawing.Size(87, 21)
        Label2.TabIndex = 51
        Label2.Text = "KREDITAS:"
        '
        'Label3
        '
        Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Label3.AutoSize = True
        Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label3.Location = New System.Drawing.Point(26, 120)
        Label3.Name = "Label3"
        Label3.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Label3.Size = New System.Drawing.Size(63, 18)
        Label3.TabIndex = 47
        Label3.Text = "Šablonas:"
        '
        'PersonAccGridComboBox
        '
        Me.PersonAccGridComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.JournalEntryBindingSource, "Person", True))
        Me.PersonAccGridComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PersonAccGridComboBox.EmptyValueString = ""
        Me.PersonAccGridComboBox.FilterString = ""
        Me.PersonAccGridComboBox.FormattingEnabled = True
        Me.PersonAccGridComboBox.InstantBinding = True
        Me.PersonAccGridComboBox.Location = New System.Drawing.Point(3, 3)
        Me.PersonAccGridComboBox.Name = "PersonAccGridComboBox"
        Me.PersonAccGridComboBox.Size = New System.Drawing.Size(717, 21)
        Me.PersonAccGridComboBox.TabIndex = 0
        '
        'JournalEntryBindingSource
        '
        Me.JournalEntryBindingSource.DataSource = GetType(ApskaitaObjects.General.JournalEntry)
        '
        'ContentTextBox
        '
        Me.ContentTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.JournalEntryBindingSource, "Content", True))
        Me.ContentTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContentTextBox.Location = New System.Drawing.Point(3, 3)
        Me.ContentTextBox.MaxLength = 254
        Me.ContentTextBox.Name = "ContentTextBox"
        Me.ContentTextBox.Size = New System.Drawing.Size(717, 20)
        Me.ContentTextBox.TabIndex = 0
        '
        'CreditSumAccBox
        '
        Me.CreditSumAccBox.BackColor = System.Drawing.Color.White
        Me.CreditSumAccBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.JournalEntryBindingSource, "CreditSum", True))
        Me.CreditSumAccBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CreditSumAccBox.KeepBackColorWhenReadOnly = False
        Me.CreditSumAccBox.Location = New System.Drawing.Point(96, 3)
        Me.CreditSumAccBox.Name = "CreditSumAccBox"
        Me.CreditSumAccBox.NegativeValue = False
        Me.CreditSumAccBox.ReadOnly = True
        Me.CreditSumAccBox.Size = New System.Drawing.Size(147, 20)
        Me.CreditSumAccBox.TabIndex = 3
        Me.CreditSumAccBox.TabStop = False
        Me.CreditSumAccBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DateDateTimePicker
        '
        Me.DateDateTimePicker.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.JournalEntryBindingSource, "Date", True))
        Me.DateDateTimePicker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DateDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateDateTimePicker.Location = New System.Drawing.Point(3, 3)
        Me.DateDateTimePicker.Name = "DateDateTimePicker"
        Me.DateDateTimePicker.Size = New System.Drawing.Size(238, 20)
        Me.DateDateTimePicker.TabIndex = 0
        '
        'DebetSumAccBox
        '
        Me.DebetSumAccBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.JournalEntryBindingSource, "DebetSum", True))
        Me.DebetSumAccBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DebetSumAccBox.KeepBackColorWhenReadOnly = False
        Me.DebetSumAccBox.Location = New System.Drawing.Point(92, 3)
        Me.DebetSumAccBox.Name = "DebetSumAccBox"
        Me.DebetSumAccBox.NegativeValue = False
        Me.DebetSumAccBox.ReadOnly = True
        Me.DebetSumAccBox.Size = New System.Drawing.Size(147, 20)
        Me.DebetSumAccBox.TabIndex = 7
        Me.DebetSumAccBox.TabStop = False
        Me.DebetSumAccBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DocNumberTextBox
        '
        Me.DocNumberTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.DocNumberTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.JournalEntryBindingSource, "DocNumber", True))
        Me.DocNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DocNumberTextBox.Location = New System.Drawing.Point(358, 3)
        Me.DocNumberTextBox.MaxLength = 20
        Me.DocNumberTextBox.Name = "DocNumberTextBox"
        Me.DocNumberTextBox.Size = New System.Drawing.Size(360, 20)
        Me.DocNumberTextBox.TabIndex = 1
        Me.DocNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DocTypeHumanReadableTextBox
        '
        Me.DocTypeHumanReadableTextBox.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.DocTypeHumanReadableTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.JournalEntryBindingSource, "DocTypeHumanReadable", True))
        Me.DocTypeHumanReadableTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DocTypeHumanReadableTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DocTypeHumanReadableTextBox.Location = New System.Drawing.Point(327, 3)
        Me.DocTypeHumanReadableTextBox.Name = "DocTypeHumanReadableTextBox"
        Me.DocTypeHumanReadableTextBox.ReadOnly = True
        Me.DocTypeHumanReadableTextBox.Size = New System.Drawing.Size(386, 20)
        Me.DocTypeHumanReadableTextBox.TabIndex = 13
        Me.DocTypeHumanReadableTextBox.TabStop = False
        Me.DocTypeHumanReadableTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'IDTextBox
        '
        Me.IDTextBox.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.IDTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.JournalEntryBindingSource, "ID", True))
        Me.IDTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IDTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IDTextBox.Location = New System.Drawing.Point(3, 3)
        Me.IDTextBox.Name = "IDTextBox"
        Me.IDTextBox.ReadOnly = True
        Me.IDTextBox.Size = New System.Drawing.Size(162, 20)
        Me.IDTextBox.TabIndex = 0
        Me.IDTextBox.TabStop = False
        Me.IDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel2
        '
        Me.Panel2.AutoSize = True
        Me.Panel2.Controls.Add(Me.ViewRelationsButton)
        Me.Panel2.Controls.Add(Me.NewItemButton)
        Me.Panel2.Controls.Add(Me.nCancelButton)
        Me.Panel2.Controls.Add(Me.ApplyButton)
        Me.Panel2.Controls.Add(Me.nOkButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 435)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.Panel2.Size = New System.Drawing.Size(835, 42)
        Me.Panel2.TabIndex = 2
        '
        'ViewRelationsButton
        '
        Me.ViewRelationsButton.Image = Global.AccDataBindingsWinForms.My.Resources.Resources.link_go_icon_24
        Me.ViewRelationsButton.Location = New System.Drawing.Point(11, 8)
        Me.ViewRelationsButton.Margin = New System.Windows.Forms.Padding(0)
        Me.ViewRelationsButton.Name = "ViewRelationsButton"
        Me.ViewRelationsButton.Size = New System.Drawing.Size(30, 30)
        Me.ViewRelationsButton.TabIndex = 1
        Me.ViewRelationsButton.UseVisualStyleBackColor = True
        '
        'NewItemButton
        '
        Me.NewItemButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewItemButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.NewItemButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NewItemButton.Location = New System.Drawing.Point(748, 12)
        Me.NewItemButton.Name = "NewItemButton"
        Me.NewItemButton.Size = New System.Drawing.Size(75, 23)
        Me.NewItemButton.TabIndex = 5
        Me.NewItemButton.Text = "Naujas"
        Me.NewItemButton.UseVisualStyleBackColor = True
        '
        'nCancelButton
        '
        Me.nCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.nCancelButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nCancelButton.Location = New System.Drawing.Point(667, 12)
        Me.nCancelButton.Name = "nCancelButton"
        Me.nCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.nCancelButton.TabIndex = 4
        Me.nCancelButton.Text = "Atšaukti"
        Me.nCancelButton.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ApplyButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplyButton.Location = New System.Drawing.Point(586, 12)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(75, 23)
        Me.ApplyButton.TabIndex = 3
        Me.ApplyButton.Text = "Taikyti"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'nOkButton
        '
        Me.nOkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nOkButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nOkButton.Location = New System.Drawing.Point(505, 12)
        Me.nOkButton.Name = "nOkButton"
        Me.nOkButton.Size = New System.Drawing.Size(75, 23)
        Me.nOkButton.TabIndex = 2
        Me.nOkButton.Text = "OK"
        Me.nOkButton.UseVisualStyleBackColor = True
        '
        'LoadTemplateButton
        '
        Me.LoadTemplateButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LoadTemplateButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LoadTemplateButton.Location = New System.Drawing.Point(687, 3)
        Me.LoadTemplateButton.Name = "LoadTemplateButton"
        Me.LoadTemplateButton.Size = New System.Drawing.Size(53, 23)
        Me.LoadTemplateButton.TabIndex = 1
        Me.LoadTemplateButton.Text = "Įkrauti"
        Me.LoadTemplateButton.UseVisualStyleBackColor = True
        '
        'TemplateComboBox
        '
        Me.TemplateComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TemplateComboBox.FormattingEnabled = True
        Me.TemplateComboBox.Location = New System.Drawing.Point(3, 3)
        Me.TemplateComboBox.Name = "TemplateComboBox"
        Me.TemplateComboBox.Size = New System.Drawing.Size(678, 21)
        Me.TemplateComboBox.TabIndex = 0
        '
        'DebetListBindingSource
        '
        Me.DebetListBindingSource.DataMember = "DebetList"
        Me.DebetListBindingSource.DataSource = Me.JournalEntryBindingSource
        '
        'CreditListBindingSource
        '
        Me.CreditListBindingSource.DataMember = "CreditList"
        Me.CreditListBindingSource.DataSource = Me.JournalEntryBindingSource
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel9, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel8, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel4, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Label3, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(ContentLabel, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(IDLabel, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(DateLabel, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(PersonLabel, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 6)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(835, 149)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel9
        '
        Me.TableLayoutPanel9.ColumnCount = 2
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel9.Controls.Add(Me.ContentTextBox, 0, 0)
        Me.TableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel9.Location = New System.Drawing.Point(92, 82)
        Me.TableLayoutPanel9.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
        Me.TableLayoutPanel9.RowCount = 1
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel9.Size = New System.Drawing.Size(743, 28)
        Me.TableLayoutPanel9.TabIndex = 3
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 6
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.DateDateTimePicker, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.DocNumberTextBox, 4, 0)
        Me.TableLayoutPanel3.Controls.Add(DocNumberLabel, 2, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(92, 27)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(743, 27)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.ColumnCount = 2
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel8.Controls.Add(Me.PersonAccGridComboBox, 0, 0)
        Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(92, 54)
        Me.TableLayoutPanel8.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 1
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(743, 28)
        Me.TableLayoutPanel8.TabIndex = 2
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.Controls.Add(Me.TemplateComboBox, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.LoadTemplateButton, 1, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(92, 120)
        Me.TableLayoutPanel4.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(743, 29)
        Me.TableLayoutPanel4.TabIndex = 4
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 6
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.IDTextBox, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.DocTypeHumanReadableTextBox, 4, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.ViewDocumentButton, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(DocTypeHumanReadableLabel, 2, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(92, 0)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(743, 27)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'ViewDocumentButton
        '
        Me.ViewDocumentButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewDocumentButton.Image = Global.AccDataBindingsWinForms.My.Resources.Resources.open_document_16
        Me.ViewDocumentButton.Location = New System.Drawing.Point(300, 0)
        Me.ViewDocumentButton.Margin = New System.Windows.Forms.Padding(0)
        Me.ViewDocumentButton.Name = "ViewDocumentButton"
        Me.ViewDocumentButton.Size = New System.Drawing.Size(24, 24)
        Me.ViewDocumentButton.TabIndex = 56
        Me.ViewDocumentButton.TabStop = False
        Me.ViewDocumentButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.TableLayoutPanel5.ColumnCount = 2
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.DebitListDataListView, 0, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.CreditListDataListView, 0, 1)
        Me.TableLayoutPanel5.Controls.Add(Me.TableLayoutPanel7, 1, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.TableLayoutPanel6, 0, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(0, 149)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 2
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(835, 286)
        Me.TableLayoutPanel5.TabIndex = 1
        '
        'DebitListDataListView
        '
        Me.DebitListDataListView.AllColumns.Add(Me.OlvColumn4)
        Me.DebitListDataListView.AllColumns.Add(Me.OlvColumn5)
        Me.DebitListDataListView.AllColumns.Add(Me.OlvColumn6)
        Me.DebitListDataListView.AllowColumnReorder = True
        Me.DebitListDataListView.AutoGenerateColumns = False
        Me.DebitListDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.DebitListDataListView.CellEditEnterChangesRows = True
        Me.DebitListDataListView.CellEditTabChangesRows = True
        Me.DebitListDataListView.CellEditUseWholeCell = False
        Me.DebitListDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn4, Me.OlvColumn5, Me.OlvColumn6})
        Me.DebitListDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.DebitListDataListView.DataSource = Me.DebetListBindingSource
        Me.DebitListDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DebitListDataListView.FullRowSelect = True
        Me.DebitListDataListView.HasCollapsibleGroups = False
        Me.DebitListDataListView.HeaderWordWrap = True
        Me.DebitListDataListView.HideSelection = False
        Me.DebitListDataListView.IncludeColumnHeadersInCopy = True
        Me.DebitListDataListView.Location = New System.Drawing.Point(5, 39)
        Me.DebitListDataListView.Name = "DebitListDataListView"
        Me.DebitListDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.DebitListDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.DebitListDataListView.SelectedBackColor = System.Drawing.Color.PaleGreen
        Me.DebitListDataListView.SelectedForeColor = System.Drawing.Color.Black
        Me.DebitListDataListView.ShowCommandMenuOnRightClick = True
        Me.DebitListDataListView.ShowGroups = False
        Me.DebitListDataListView.ShowImagesOnSubItems = True
        Me.DebitListDataListView.ShowItemCountOnGroups = True
        Me.DebitListDataListView.ShowItemToolTips = True
        Me.DebitListDataListView.Size = New System.Drawing.Size(408, 242)
        Me.DebitListDataListView.TabIndex = 0
        Me.DebitListDataListView.UseCellFormatEvents = True
        Me.DebitListDataListView.UseCompatibleStateImageBehavior = False
        Me.DebitListDataListView.UseFilterIndicator = True
        Me.DebitListDataListView.UseFiltering = True
        Me.DebitListDataListView.UseHotItem = True
        Me.DebitListDataListView.UseNotifyPropertyChanged = True
        Me.DebitListDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn4
        '
        Me.OlvColumn4.AspectName = "Account"
        Me.OlvColumn4.CellEditUseWholeCell = True
        Me.OlvColumn4.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn4.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn4.Text = "Sąskaita"
        Me.OlvColumn4.ToolTipText = ""
        Me.OlvColumn4.Width = 87
        '
        'OlvColumn5
        '
        Me.OlvColumn5.AspectName = "Amount"
        Me.OlvColumn5.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn5.CellEditUseWholeCell = True
        Me.OlvColumn5.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn5.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn5.Text = "Suma"
        Me.OlvColumn5.ToolTipText = ""
        Me.OlvColumn5.Width = 90
        '
        'OlvColumn6
        '
        Me.OlvColumn6.AspectName = "Person"
        Me.OlvColumn6.CellEditUseWholeCell = True
        Me.OlvColumn6.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn6.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn6.Text = "Susietas asmuo"
        Me.OlvColumn6.ToolTipText = ""
        Me.OlvColumn6.Width = 280
        '
        'CreditListDataListView
        '
        Me.CreditListDataListView.AllColumns.Add(Me.OlvColumn1)
        Me.CreditListDataListView.AllColumns.Add(Me.OlvColumn2)
        Me.CreditListDataListView.AllColumns.Add(Me.OlvColumn3)
        Me.CreditListDataListView.AllowColumnReorder = True
        Me.CreditListDataListView.AutoGenerateColumns = False
        Me.CreditListDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.CreditListDataListView.CellEditEnterChangesRows = True
        Me.CreditListDataListView.CellEditTabChangesRows = True
        Me.CreditListDataListView.CellEditUseWholeCell = False
        Me.CreditListDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn1, Me.OlvColumn2, Me.OlvColumn3})
        Me.CreditListDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.CreditListDataListView.DataSource = Me.CreditListBindingSource
        Me.CreditListDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CreditListDataListView.FullRowSelect = True
        Me.CreditListDataListView.HasCollapsibleGroups = False
        Me.CreditListDataListView.HeaderWordWrap = True
        Me.CreditListDataListView.HideSelection = False
        Me.CreditListDataListView.IncludeColumnHeadersInCopy = True
        Me.CreditListDataListView.Location = New System.Drawing.Point(421, 39)
        Me.CreditListDataListView.Name = "CreditListDataListView"
        Me.CreditListDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.CreditListDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.CreditListDataListView.ShowCommandMenuOnRightClick = True
        Me.CreditListDataListView.ShowGroups = False
        Me.CreditListDataListView.ShowImagesOnSubItems = True
        Me.CreditListDataListView.ShowItemCountOnGroups = True
        Me.CreditListDataListView.ShowItemToolTips = True
        Me.CreditListDataListView.Size = New System.Drawing.Size(409, 242)
        Me.CreditListDataListView.TabIndex = 1
        Me.CreditListDataListView.UseCellFormatEvents = True
        Me.CreditListDataListView.UseCompatibleStateImageBehavior = False
        Me.CreditListDataListView.UseFilterIndicator = True
        Me.CreditListDataListView.UseFiltering = True
        Me.CreditListDataListView.UseHotItem = True
        Me.CreditListDataListView.UseNotifyPropertyChanged = True
        Me.CreditListDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn1
        '
        Me.OlvColumn1.AspectName = "Account"
        Me.OlvColumn1.CellEditUseWholeCell = True
        Me.OlvColumn1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn1.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn1.Text = "Sąskaita"
        Me.OlvColumn1.ToolTipText = ""
        Me.OlvColumn1.Width = 89
        '
        'OlvColumn2
        '
        Me.OlvColumn2.AspectName = "Amount"
        Me.OlvColumn2.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn2.CellEditUseWholeCell = True
        Me.OlvColumn2.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn2.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn2.Text = "Suma"
        Me.OlvColumn2.ToolTipText = ""
        Me.OlvColumn2.Width = 96
        '
        'OlvColumn3
        '
        Me.OlvColumn3.AspectName = "Person"
        Me.OlvColumn3.CellEditUseWholeCell = True
        Me.OlvColumn3.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn3.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn3.Text = "Susietas asmuo"
        Me.OlvColumn3.ToolTipText = ""
        Me.OlvColumn3.Width = 281
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.AutoSize = True
        Me.TableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel7.ColumnCount = 2
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.CreditSumAccBox, 1, 0)
        Me.TableLayoutPanel7.Controls.Add(Label2, 0, 0)
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(421, 5)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 1
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(409, 26)
        Me.TableLayoutPanel7.TabIndex = 5
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.AutoSize = True
        Me.TableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel6.ColumnCount = 2
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel6.Controls.Add(Label1, 0, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.DebetSumAccBox, 1, 0)
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(5, 5)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 1
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(408, 26)
        Me.TableLayoutPanel6.TabIndex = 5
        '
        'ProgressFiller1
        '
        Me.ProgressFiller1.Location = New System.Drawing.Point(286, 188)
        Me.ProgressFiller1.Name = "ProgressFiller1"
        Me.ProgressFiller1.Size = New System.Drawing.Size(422, 191)
        Me.ProgressFiller1.TabIndex = 52
        Me.ProgressFiller1.Visible = False
        '
        'ProgressFiller2
        '
        Me.ProgressFiller2.Location = New System.Drawing.Point(178, 320)
        Me.ProgressFiller2.Name = "ProgressFiller2"
        Me.ProgressFiller2.Size = New System.Drawing.Size(422, 191)
        Me.ProgressFiller2.TabIndex = 53
        Me.ProgressFiller2.Visible = False
        '
        'ErrorWarnInfoProvider1
        '
        Me.ErrorWarnInfoProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.BlinkStyleInformation = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.BlinkStyleWarning = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.ContainerControl = Me
        Me.ErrorWarnInfoProvider1.DataSource = Me.JournalEntryBindingSource
        '
        'F_JournalEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(835, 477)
        Me.Controls.Add(Me.TableLayoutPanel5)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.ProgressFiller1)
        Me.Controls.Add(Me.ProgressFiller2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "F_JournalEntry"
        Me.ShowInTaskbar = False
        Me.Text = "Bendrojo žurnalo įrašas"
        CType(Me.JournalEntryBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        CType(Me.DebetListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreditListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel9.ResumeLayout(False)
        Me.TableLayoutPanel9.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        CType(Me.DebitListDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreditListDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.TableLayoutPanel7.PerformLayout()
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.TableLayoutPanel6.PerformLayout()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents JournalEntryBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CreditSumAccBox As AccControlsWinForms.AccTextBox
    Friend WithEvents DateDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents DebetSumAccBox As AccControlsWinForms.AccTextBox
    Friend WithEvents DocNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DocTypeHumanReadableTextBox As System.Windows.Forms.TextBox
    Friend WithEvents IDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NewItemButton As System.Windows.Forms.Button
    Friend WithEvents nCancelButton As System.Windows.Forms.Button
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents nOkButton As System.Windows.Forms.Button
    Friend WithEvents LoadTemplateButton As System.Windows.Forms.Button
    Friend WithEvents TemplateComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents DebetListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CreditListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents PersonAccGridComboBox As AccControlsWinForms.AccListComboBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel6 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel9 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel8 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ViewDocumentButton As System.Windows.Forms.Button
    Friend WithEvents ViewRelationsButton As System.Windows.Forms.Button
    Friend WithEvents DebitListDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents CreditListDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents OlvColumn1 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn2 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn3 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn4 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn5 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn6 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents ProgressFiller1 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ProgressFiller2 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ErrorWarnInfoProvider1 As AccControlsWinForms.ErrorWarnInfoProvider
End Class
