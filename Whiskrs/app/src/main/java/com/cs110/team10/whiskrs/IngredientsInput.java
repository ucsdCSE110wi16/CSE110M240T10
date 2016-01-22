package com.cs110.team10.whiskrs;

import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.goebl.david.Request;
import com.goebl.david.Response;
import com.goebl.david.Webb;

import org.json.JSONArray;
import org.json.JSONObject;

import java.net.HttpURLConnection;
import java.net.URL;

public class IngredientsInput extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ingredients_input);


        Button search = (Button)findViewById(R.id.searchIngredients);
        search.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                EditText in = (EditText)findViewById(R.id.editText);
                String text = in.getText().toString();
                clearInput(in);
                TextView out = (TextView)findViewById(R.id.output);
            }
        });
    }

    private void clearInput(EditText in){
        in.setText("");
    }
}
