﻿#pragma checksum "..\..\SelectOnMovieWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9CA01B569C128A0BBA6E5725DBF32A2AF1677AA96AA5C05B79D1D5EF053179C1"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CommonUser;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CommonUser {
    
    
    /// <summary>
    /// SelectOnMovieWindow
    /// </summary>
    public partial class SelectOnMovieWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 9 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Grid_SelectOnMovie;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Tab_Today;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListView_OnMoviesToday;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Tab_Tomorrow;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListView_OnMoviesTomo;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem Tab_AfterTom;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\SelectOnMovieWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListView_OnMoviesAfTomo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CommonUser;component/selectonmoviewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SelectOnMovieWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Grid_SelectOnMovie = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.Tab_Today = ((System.Windows.Controls.TabItem)(target));
            
            #line 22 "..\..\SelectOnMovieWindow.xaml"
            this.Tab_Today.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Tab_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ListView_OnMoviesToday = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.Tab_Tomorrow = ((System.Windows.Controls.TabItem)(target));
            return;
            case 5:
            this.ListView_OnMoviesTomo = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.Tab_AfterTom = ((System.Windows.Controls.TabItem)(target));
            return;
            case 7:
            this.ListView_OnMoviesAfTomo = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            
            #line 96 "..\..\SelectOnMovieWindow.xaml"
            ((System.Windows.Controls.Image)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.X_MouseEnter);
            
            #line default
            #line hidden
            
            #line 96 "..\..\SelectOnMovieWindow.xaml"
            ((System.Windows.Controls.Image)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.X_MouseLeave);
            
            #line default
            #line hidden
            
            #line 96 "..\..\SelectOnMovieWindow.xaml"
            ((System.Windows.Controls.Image)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Image_Back_MouseDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 9:
            
            #line 113 "..\..\SelectOnMovieWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.X_MouseEnter);
            
            #line default
            #line hidden
            
            #line 114 "..\..\SelectOnMovieWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.X_MouseLeave);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

