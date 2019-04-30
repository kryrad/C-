import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Random;

import static java.lang.Math.abs;

class MainWindow extends JFrame implements ActionListener {
    private JPanel rootPanel;
    private JButton button1;
    Random random = new Random();
    double width, height, v=4.0, direction,X,Y;

    Timer timer = new Timer(10, this);
    MainWindow(){
        button1.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                random=new Random();
                X=button1.getLocation().x;
                Y=button1.getLocation().y;
                width=(random.nextInt( rootPanel.getWidth() -70 + 1)-button1.getLocation().x);
                height=(random.nextInt(rootPanel.getHeight() -70 + 1)-button1.getLocation().y);
                direction=Math.atan2(height,width);
                timer.start();
            }
        });
    }
    public static void main(String[] args) {
        final JFrame frame = new JFrame();
        frame.setContentPane((new MainWindow()).rootPanel);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setUndecorated(true);
        frame.setExtendedState(JFrame.MAXIMIZED_BOTH);
        frame.setVisible(true);
        frame.setBackground(new Color(0, 0, 0, 0));
    }

    public void actionPerformed(ActionEvent e) {
        if ((abs(button1.getLocation().x-width-X)>10)&&(abs(button1.getLocation().y-height-Y)>10)) {
            button1.setLocation((int)(button1.getLocation().x + (v * Math.cos(direction))),(int)(button1.getLocation().y + (v * Math.sin(direction))));
            button1.repaint();
        }
        else{
            timer.stop();
        }
    }
}