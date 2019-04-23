import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Random;

class MainWindow extends JFrame {
    private JPanel rootPanel;
    private JButton button1;
    MainWindow(){
        button1.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                Random random = new Random();
                button1.setBounds(random.nextInt(rootPanel.getWidth() -70 + 1),random.nextInt(rootPanel.getHeight() -70 + 1),70,70);
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

}